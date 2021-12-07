using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspnetRunBasics.DTOs;

namespace AspnetRunBasics.HttpClients
{
    public interface IBasketClient
    {
        Task<BasketDto> GetBasketAsync(string userName, CancellationToken cancellationToken = default);
        Task<BasketDto> UpsertBasketAsync(BasketDto basket, CancellationToken cancellationToken = default);
        Task<bool> CheckoutBasketAsync(BasketCheckoutDto basket, CancellationToken cancellationToken = default);
    }
}
