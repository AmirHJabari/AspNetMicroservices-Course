using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Grpc
{
    public class GrpcSettings : IGrpcSettings
    {
        public string DiscountGrpcUrl { get; set; }
    }
}
