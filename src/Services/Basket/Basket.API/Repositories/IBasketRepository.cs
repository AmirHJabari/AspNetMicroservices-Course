using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Basket.API.Entities;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart> GetBasketAsync(string username, CancellationToken cancellationToken = default);
        Task SetBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default);
        Task DeleteBasketAsync(string username, CancellationToken cancellationToken = default);
    }
}
