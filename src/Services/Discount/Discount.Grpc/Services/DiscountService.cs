using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using grpc = Discount.Grpc.Protos;

namespace Discount.Grpc.Services
{
    public class DiscountService : grpc.Discount.DiscountBase
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;
        public DiscountService(ILogger<DiscountService> logger, IDiscountRepository discountRepository, IMapper mapper)
        {
            this._logger = logger;
            this._discountRepository = discountRepository;
            this._mapper = mapper;
        }

        public override async Task<CouponModel> GetById(DiscountById request, ServerCallContext context)
        {
            var coupon = await this._discountRepository.GetCouponAsync(request.Id, context.CancellationToken);
            if (coupon is null)
                throw new RpcException(new Status(
                    StatusCode.NotFound,
                    $"There is no discount with id of '{request.Id}'."));

            return _mapper.Map<CouponModel>(coupon);
        }
        public override async Task<CouponModel> GetByProductId(DiscountByProductId request, ServerCallContext context)
        {
            var coupon = await this._discountRepository.GetCouponAsync(request.ProductId, context.CancellationToken);
            if (coupon is null)
                throw new RpcException(new Status(
                    StatusCode.NotFound,
                    $"There is no discount with productId of '{request.ProductId}'."));

            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<ManyDiscountModel> GetManyDiscountsById(ManyDiscountById request, ServerCallContext context)
        {
            var result = new ManyDiscountModel();

            foreach (var id in request.Ids)
            {
                var discount = await this._discountRepository.GetDiscountAmountAsync(id, context.CancellationToken);
                result.Amounts.Add((float)discount);
            }

            return result;
        }
        public override async Task<ManyDiscountModel> GetManyDiscountsByProductId(ManyDiscountByProductId request, ServerCallContext context)
        {
            var result = new ManyDiscountModel();

            foreach (var productId in request.ProductIds)
            {
                var discount = await this._discountRepository.GetDiscountAmountAsync(productId, context.CancellationToken);
                result.Amounts.Add((float)discount);
            }

            return result;
        }

        public override async Task<ObjectResult> Create(CouponModel request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request);
            bool success = await this._discountRepository.CreateCouponAsync(coupon, context.CancellationToken);

            return new ObjectResult() { Success = success };
        }
        public override async Task<ObjectResult> Update(CouponModel request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request);
            bool success = await this._discountRepository.UpdateCouponAsync(coupon, context.CancellationToken);

            return new ObjectResult() { Success = success };
        }

        public override async Task<ObjectResult> DeleteById(DiscountById request, ServerCallContext context)
        {
            bool success = await this._discountRepository.DeleteCouponAsync(request.Id, context.CancellationToken);
            return new ObjectResult() { Success = success };
        }
        public override async Task<ObjectResult> DeleteProductId(DiscountByProductId request, ServerCallContext context)
        {
            bool success = await this._discountRepository.DeleteCouponAsync(request.ProductId, context.CancellationToken);
            return new ObjectResult() { Success = success };
        }
    }
}
