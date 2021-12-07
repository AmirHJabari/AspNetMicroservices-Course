using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Settings
{
    public interface IUrlsSettings
    {
        string CatalogBaseUrl { get; set;}
        string BasketBaseUrl { get; set;}
        string OrderingBaseUrl { get; set;}
    }
}
