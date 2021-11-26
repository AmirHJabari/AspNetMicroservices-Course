using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.API.Entities;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken = default);
        Task<Product> GetProductAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetProductsAsync(string name, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName, CancellationToken cancellationToken = default);

        Task InsertProductAsync(Product product, CancellationToken cancellationToken = default);
        Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
        Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken = default);
    }
}
