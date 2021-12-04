namespace Basket.API
{
    public class MassTransitSettings : IMassTransitSettings
    {
        public string RabbitMqHostUrl { get; set; }
    }
}