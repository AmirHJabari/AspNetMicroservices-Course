namespace Discount.API.Data
{
    public interface IPostgresSettings
    {
        /// <summary>
        /// The connection string for the postgres server.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The name of the main database.
        /// </summary>
        public string DiscountDbName { get; set; }

        /// <summary>
        /// The name of the coupon table.
        /// </summary>
        public string CouponTblName { get; set; }
    }
}
