using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspnetRunBasics.DTOs;

namespace AspnetRunBasics.HttpClients
{
    public interface ICatalogClient
    {
        Task<IEnumerable<CatalogDto>> GetCatalogsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<CatalogDto>> GetCatalogsByCategoryAsync(string categoryName, CancellationToken cancellationToken = default);
        Task<CatalogDto> GetCatalogByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<CatalogDto> CreateCatalogAsync(CatalogDto catalog, CancellationToken cancellationToken = default);
    }
}
