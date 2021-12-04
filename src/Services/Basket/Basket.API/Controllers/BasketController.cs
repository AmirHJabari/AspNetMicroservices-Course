using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Basket.API.Repositories;
using Basket.API.Entities;
using System.Threading;
using Basket.API.Grpc;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using Basket.API.DTOs;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IDiscountGrpcClient _discountClient;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public BasketController(IBasketRepository basketRepository, IDiscountGrpcClient discountClient, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            this._basketRepository = basketRepository;
            this._discountClient = discountClient;
            this._mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<ShoppingCart>> Get(string username, CancellationToken cancellationToken)
        {
            var basket = await this._basketRepository.GetBasketAsync(username, cancellationToken);
            return Ok(basket ?? new ShoppingCart(username));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> Upsert([FromBody] ShoppingCart basket, CancellationToken cancellationToken)
        {
            if (basket.Items.Any())
            {
                var discounts = await _discountClient.GetManyDiscountAmountAsync(basket.Items.Select(i => i.ProductId), cancellationToken);

                for (int i = 0; i < discounts.Amounts.Count; i++)
                    basket.Items[i].Price -= (decimal)discounts.Amounts[i];
            }

            await this._basketRepository.SetBasketAsync(basket, cancellationToken);
            return CreatedAtAction(nameof(Get), new { basket.UserName }, basket);
        }

        [HttpDelete("{username}")]
        public async Task<ActionResult> Delete(string username, CancellationToken cancellationToken)
        {
            await this._basketRepository.DeleteBasketAsync(username, cancellationToken);
            return Ok();
        }

        [HttpPost("[action]")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckoutDto basketCheckout, CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.GetBasketAsync(basketCheckout.UserName, cancellationToken);
            if (basket is null)
                return BadRequest();
            
            var basketCheckoutEvent = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            basketCheckoutEvent.TotalPrice = basket.BasketPrice;

            await _publishEndpoint.Publish(basketCheckoutEvent, cancellationToken);

            await this._basketRepository.DeleteBasketAsync(basketCheckout.UserName, cancellationToken);

            return Accepted();
        }
    }
}
