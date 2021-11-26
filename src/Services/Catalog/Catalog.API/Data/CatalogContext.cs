using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(IMongoSettings mongoSettings)
        {
            var client = new MongoClient(mongoSettings.ConnectionString);
            var database = client.GetDatabase(mongoSettings.DatabaseName);
            Products = database.GetCollection<Product>(mongoSettings.CollectionName);
        }

        public IMongoCollection<Product> Products { get; }
    }
}
