using Microsoft.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AspnetRunBasics.DTOs;

namespace AspnetRunBasics.HttpClients
{
    public class CatalogClient : ICatalogClient
    {
        private readonly HttpClient _client;

        public CatalogClient(HttpClient client)
        {
            this._client = client;
        }

        public async Task<CatalogDto> CreateCatalogAsync(CatalogDto catalog, CancellationToken cancellationToken = default)
        {
            var res = await _client.PostAsJsonAsync("/api/v1/Catalog", catalog, cancellationToken);
            return await res.Content.ReadFromJsonAsync<CatalogDto>(cancellationToken: cancellationToken);
        }

        public Task<CatalogDto> GetCatalogByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return _client.GetFromJsonAsync<CatalogDto>($"/api/v1/catalog/{id}", cancellationToken);
        }

        public Task<IEnumerable<CatalogDto>> GetCatalogsAsync(CancellationToken cancellationToken = default)
        {
            return _client.GetFromJsonAsync<IEnumerable<CatalogDto>>("/api/v1/catalog", cancellationToken);
        }

        public Task<IEnumerable<CatalogDto>> GetCatalogsByCategoryAsync(string categoryName, CancellationToken cancellationToken = default)
        {
            return _client.GetFromJsonAsync<IEnumerable<CatalogDto>>($"/api/v1/catalog/GetByCategoryName/{categoryName}",
                                                    cancellationToken);
        }
    }
}
