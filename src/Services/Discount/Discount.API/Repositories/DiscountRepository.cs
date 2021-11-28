using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discount.API.Entities;
using Discount.API.Data;
using System.Threading;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IPostgresSettings _settings;
        private string CouponTbl { get; }

        public DiscountRepository(IPostgresSettings settings)
        {
            this._settings = settings;
            this.CouponTbl = settings.CouponTblName;
        }

        private NpgsqlConnection NewConnection() => new(_settings.ConnectionString);

        public async Task<bool> CreateCouponAsync(Coupon coupon, CancellationToken cancellationToken = default)
        {
            using var conn = NewConnection();
            int affected = await conn.ExecuteAsync(new CommandDefinition(
                $"INSERT INTO {CouponTbl} (ProductId, ProductName, Description, Amount) VALUES (@ProductId, @ProductName, @Description, @Amount)",
                            parameters: coupon,
                            cancellationToken: cancellationToken)
                );

            return affected > 0;
        }

        public async Task<bool> DeleteCouponAsync(string productId, CancellationToken cancellationToken = default)
        {
            using var conn = NewConnection();

            int affected = await conn.ExecuteAsync(new CommandDefinition(
                        $"DELETE FROM {CouponTbl} WHERE ProductId = @productId",
                                new { productId },
                                cancellationToken: cancellationToken)
                );

            return affected > 0;
        }

        public async Task<bool> DeleteCouponAsync(int id, CancellationToken cancellationToken = default)
        {
            using var conn = NewConnection();
            int affected = await conn.ExecuteAsync(new CommandDefinition(
                                $"DELETE FROM {CouponTbl} WHERE Id = @id",
                                new { id },
                                cancellationToken: cancellationToken)
                );

            return affected > 0;
        }

        public async Task<Coupon> GetCouponAsync(int id, CancellationToken cancellationToken = default)
        {
            using var conn = NewConnection();
            return await conn.QueryFirstOrDefaultAsync<Coupon>(new CommandDefinition(
                                $"SELECT * FROM {CouponTbl} WHERE Id = @id;",
                                new { id },
                                cancellationToken: cancellationToken)
                );
        }

        public async Task<Coupon> GetCouponAsync(string productId, CancellationToken cancellationToken = default)
        {
            using var conn = NewConnection();
            return await conn.QueryFirstOrDefaultAsync<Coupon>(new CommandDefinition(
                                $"SELECT * FROM {CouponTbl} WHERE ProductId = @productId;",
                                new { productId },
                                cancellationToken: cancellationToken)
                );
        }

        public async Task<bool> UpdateCouponAsync(Coupon coupon, CancellationToken cancellationToken = default)
        {
            using var conn = NewConnection();
            int affected = await conn.ExecuteAsync(new CommandDefinition(
                                $"UPDATE {CouponTbl} SET ProductId=@ProductId, ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id = @Id",
                                coupon,
                                cancellationToken: cancellationToken)
                );

            return affected > 0;
        }
    }
}
