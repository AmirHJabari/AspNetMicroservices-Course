using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AspnetRunBasics.DTOs;

namespace AspnetRunBasics.HttpClients
{
    public class BasketClient : IBasketClient
    {
        private readonly HttpClient _client;

        public BasketClient(HttpClient client)
        {
            this._client = client;
        }

        public async Task<bool> CheckoutBasketAsync(BasketCheckoutDto basket, CancellationToken cancellationToken = default)
        {
            var res = await _client.PostAsJsonAsync("/api/v1/Basket/Checkout", basket, cancellationToken);
            return res.IsSuccessStatusCode;
        }

        // More info https://www.stevejgordon.co.uk/sending-and-receiving-json-using-httpclient-with-system-net-http-json
        public Task<BasketDto> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            try
            {
                return _client.GetFromJsonAsync<BasketDto>($"/api/v1/Basket/{userName}", cancellationToken);
            }
            catch (HttpRequestException) // Non success
            {
                Console.WriteLine("An error occurred.");
            }
            catch (NotSupportedException) // When content type is not valid
            {
                Console.WriteLine("The content type is not supported.");
            }
            catch (JsonException) // Invalid JSON
            {
                Console.WriteLine("Invalid JSON.");
            }
            return null;
        }

        public async Task<BasketDto> UpsertBasketAsync(BasketDto basket, CancellationToken cancellationToken = default)
        {
            var res = await _client.PostAsJsonAsync("/api/v1/Basket", basket, cancellationToken);
            return await res.Content.ReadFromJsonAsync<BasketDto>(cancellationToken: cancellationToken);
        }
    }
}
