namespace Discount.API.Data
{
    public class PostgresSettings : IPostgresSettings
    {
        public string ConnectionString { get; set; }
        public string DiscountDbName { get; set; }
        public string CouponTblName { get; set; }
    }
}
