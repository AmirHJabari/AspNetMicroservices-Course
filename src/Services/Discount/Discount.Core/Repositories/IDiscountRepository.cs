using System.Threading;
using System.Threading.Tasks;
using Discount.Core.Entities;

namespace Discount.Core.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetCouponAsync(int id, CancellationToken cancellationToken = default);
        Task<Coupon> GetCouponAsync(string productId, CancellationToken cancellationToken = default);

        Task<bool> CreateCouponAsync(Coupon coupon, CancellationToken cancellationToken = default);
        Task<bool> UpdateCouponAsync(Coupon coupon, CancellationToken cancellationToken = default);

        Task<bool> DeleteCouponAsync(string productId, CancellationToken cancellationToken = default);
        Task<bool> DeleteCouponAsync(int id, CancellationToken cancellationToken = default);
    }
}
