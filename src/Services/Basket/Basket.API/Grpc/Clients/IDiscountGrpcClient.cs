﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discount.Grpc.Protos;

namespace Basket.API.Grpc
{
    public interface IDiscountGrpcClient
    {
        Task<CouponModel> GetDiscountAsync(string productId, CancellationToken cancellationToken = default);
        Task<ManyDiscountModel> GetManyDiscountAmountAsync(IEnumerable<string> productIds, CancellationToken cancellationToken = default);
    }
}
