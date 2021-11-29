using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Grpc
{
    public interface IGrpcSettings
    {
        string DiscountGrpcUrl { get; set; }
    }
}
