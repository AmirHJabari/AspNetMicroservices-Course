using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            this._context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> DeleteProductAsync(string id, CancellationToken cancellationToken = default)
        {
            var replaceResult = await this._context.Products.DeleteOneAsync(p => p.Id == id, cancellationToken);

            return replaceResult.IsAcknowledged &&
                   replaceResult.DeletedCount > 0;
        }

        public async Task<Product> GetProductAsync(string id, CancellationToken cancellationToken = default)
        {
            return await (await this._context.Products.FindAsync(p => p.Id == id, cancellationToken: cancellationToken))
                                    .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(CancellationToken cancellationToken = default)
        {
            return await (await this._context.Products.FindAsync(p => true, cancellationToken: cancellationToken))
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string name, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Product>.Filter.Regex(p => p.Name, name);
            return await (await this._context.Products.FindAsync(filter, cancellationToken: cancellationToken))
                            .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryName, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Product>.Filter.Regex(p => p.Category, categoryName);

            return await (await this._context.Products.FindAsync<Product>(filter, cancellationToken: cancellationToken))
                            .ToListAsync(cancellationToken);
        }

        public Task InsertProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            return this._context.Products.InsertOneAsync(product, null, cancellationToken);
        }

        public async Task<bool> UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            var replaceResult = await this._context.Products.ReplaceOneAsync(p => p.Id == product.Id, product,
                cancellationToken: cancellationToken);

            return replaceResult.IsAcknowledged &&
                   replaceResult.ModifiedCount > 0;
        }
    }
}
