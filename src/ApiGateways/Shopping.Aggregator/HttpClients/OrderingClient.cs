using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Shopping.Aggregator.DTOs;

namespace Shopping.Aggregator.HttpClients
{
    public class OrderingClient : IOrderingClient
    {
        private readonly HttpClient _client;

        public OrderingClient(HttpClient client)
        {
            this._client = client;
        }

        public Task<IEnumerable<OrderDto>> GetOrdersByUserNameAsync(string userName, CancellationToken cancellationToken = default)
        {
            return _client.GetFromJsonAsync<IEnumerable<OrderDto>>($"/api/v1/Order?userName={userName}", cancellationToken);
        }
    }
}
