using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Settings
{
    public class UrlsSettings : IUrlsSettings
    {
        public string CatalogBaseUrl { get; set; }
        public string BasketBaseUrl { get; set; }
        public string OrderingBaseUrl { get; set; }
    }
}
