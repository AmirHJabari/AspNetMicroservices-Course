using System.Threading;
using System.Threading.Tasks;
using Discount.Core.Entities;

namespace Discount.Core.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetCouponAsync(int id, CancellationToken cancellationToken = default);
        Task<Coupon> GetCouponAsync(string productId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the amount of the discount with <paramref name="productId"/>.
        /// </summary>
        /// <param name="productId">The product id of discount.</param>
        /// <returns>The amount of discount if exists, otherwise 0.</returns>
        Task<decimal> GetDiscountAmountAsync(string productId, CancellationToken cancellationToken = default);
        /// <summary>
        /// Gets the amount of the discount with <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of discount.</param>
        /// <returns>The amount of discount if exists, otherwise 0.</returns>
        Task<decimal> GetDiscountAmountAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> CreateCouponAsync(Coupon coupon, CancellationToken cancellationToken = default);
        Task<bool> UpdateCouponAsync(Coupon coupon, CancellationToken cancellationToken = default);

        Task<bool> DeleteCouponAsync(string productId, CancellationToken cancellationToken = default);
        Task<bool> DeleteCouponAsync(int id, CancellationToken cancellationToken = default);
    }
}
