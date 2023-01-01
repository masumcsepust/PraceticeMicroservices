using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Interfaces;
using Discount.Grpc.Protos;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService: DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscount _discount;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscount discount, ILogger logger, IMapper mapper)
        {
            _discount = discount ?? throw new ArgumentNullException(nameof(discount));
            _logger = logger ?? throw new ArgumentNullException(nameof(discount));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(discount));
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _discount.GetDiscount(request.ProductName);

            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount With ProductName={request.ProductName} is not found."));
            }

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _discount.CreateDiscount(coupon);

            _logger.LogInformation($"Discount is successfully created. ProductName: {coupon.ProductName}");

            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
            // return base.CreateDiscount(request, context);
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _discount.UpdateDiscount(coupon);

            _logger.LogInformation($"Discount is successfully updated. ProductName : {coupon.ProductName}");
            var couponModel = _mapper.Map<CouponModel>(coupon);

            return couponModel;

            //var deleted = new DeleteDiscountResponse
            //{
            //    Success = coupon
            //};
            //return base.UpdateDiscount(request, context);
        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _discount.DeleteDiscount(request.ProductName);

            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };

            return response;
            //return base.DeleteDiscount(request, context);
        }
    }
}
