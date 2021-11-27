namespace Basket.API.Services
{
    public static class CachingKeyProvider
    {
        public const string BasketFormat = "baskets.{0}";

        public static string GetBasketKey(string username) => string.Format(BasketFormat, username);
    }
}
