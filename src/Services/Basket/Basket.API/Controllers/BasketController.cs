using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Basket.API.Repositories;
using Basket.API.Entities;
using System.Threading;
using Basket.API.Grpc;

namespace Basket.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IDiscountGrpcClient _discountClient;

        public BasketController(IBasketRepository basketRepository, IDiscountGrpcClient discountClient)
        {
            this._basketRepository = basketRepository;
            this._discountClient = discountClient;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<ShoppingCart>> Get(string username, CancellationToken cancellationToken)
        {
            var basket = await this._basketRepository.GetBasketAsync(username, cancellationToken);
            return Ok(basket ?? new ShoppingCart(username));
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> Post([FromBody] ShoppingCart basket, CancellationToken cancellationToken)
        {
            foreach (var item in basket.Items)
            {
                var discount = await _discountClient.GetDiscount(item.ProductId, cancellationToken);
                if (discount is not null)
                    item.Price -= (decimal) discount.Amount;
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
    }
}
