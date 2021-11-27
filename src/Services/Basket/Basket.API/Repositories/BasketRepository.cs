using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Basket.API.Services;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _cache;

        public BasketRepository(IDistributedCache cache)
        {
            this._cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public Task DeleteBasketAsync(string username, CancellationToken cancellationToken = default)
        {
            string key = CachingKeyProvider.GetBasketKey(username);
            return _cache.RemoveAsync(key, cancellationToken);
        }

        public async Task<ShoppingCart> GetBasketAsync(string username, CancellationToken cancellationToken = default)
        {
            string key = CachingKeyProvider.GetBasketKey(username);
            var basketJson = await _cache.GetAsync(key, cancellationToken);
            if (basketJson is null)
                return null;

            return JsonSerializer.Deserialize<ShoppingCart>(basketJson);
        }

        public Task SetBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            byte[] basketJson = JsonSerializer.SerializeToUtf8Bytes(basket);
            string key = CachingKeyProvider.GetBasketKey(basket.UserName);

            return _cache.SetAsync(key, basketJson, token: cancellationToken);
        }
    }
}
