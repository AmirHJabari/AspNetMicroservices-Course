using Shopping.Aggregator.HttpClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.DTOs;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Threading;
using AutoMapper;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShoppingController : ControllerBase
    {
        private readonly ICatalogClient _catalogClient;
        private readonly IBasketClient _basketClient;
        private readonly IOrderingClient _orderingClient;
        private readonly IMapper _mapper;

        public ShoppingController(ICatalogClient catalogClient, IBasketClient basketClient, IOrderingClient orderingClient, IMapper mapper)
        {
            this._catalogClient = catalogClient;
            this._basketClient = basketClient;
            this._orderingClient = orderingClient;
            this._mapper = mapper;
        }

        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ShoppingDto>> Get(string username, CancellationToken cancellationToken)
        {
            var basket = await _basketClient.GetBasketAsync(username, cancellationToken);

            foreach (var item in basket.Items)
            {
                var product = await _catalogClient.GetCatalogByIdAsync(item.ProductId);
                _mapper.Map(product, item);
            }

            var orders = await _orderingClient.GetOrdersByUserNameAsync(username, cancellationToken);

            var shoppingDto = new ShoppingDto()
            {
                UserName = username,
                Basket = basket,
                Orders = orders
            };

            return Ok(shoppingDto);
        }
    }
}
