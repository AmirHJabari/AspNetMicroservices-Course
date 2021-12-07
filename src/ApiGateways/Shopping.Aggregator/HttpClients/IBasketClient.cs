using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Shopping.Aggregator.DTOs;

namespace Shopping.Aggregator.HttpClients
{
    public interface IBasketClient
    {
        Task<BasketDto> GetBasketAsync(string userName, CancellationToken cancellationToken = default);
    }
}
