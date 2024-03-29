﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discount.Grpc.Protos;
using Grpc.Core;
using grpc = Discount.Grpc.Protos;

namespace Basket.API.Grpc
{
    public class DiscountGrpcClient : IDiscountGrpcClient
    {
        private readonly grpc.Discount.DiscountClient _discountClient;

        public DiscountGrpcClient(grpc.Discount.DiscountClient discountClient)
        {
            this._discountClient = discountClient;
        }

        public async Task<CouponModel> GetDiscountAsync(string productId, CancellationToken cancellationToken = default)
        {
            var request = new DiscountByProductId() { ProductId = productId };
            try
            {
                return await _discountClient.GetByProductIdAsync(request, cancellationToken: cancellationToken);
            }
            catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<ManyDiscountModel> GetManyDiscountAmountAsync(IEnumerable<string> productIds, CancellationToken cancellationToken = default)
        {
            var request = new ManyDiscountByProductId();
            request.ProductIds.AddRange(productIds);

            return await _discountClient.GetManyDiscountsByProductIdAsync(request, cancellationToken: cancellationToken);
        }
    }
}
