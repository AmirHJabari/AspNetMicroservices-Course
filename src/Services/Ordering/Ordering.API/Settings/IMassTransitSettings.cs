namespace Ordering.API
{
    public interface IMassTransitSettings
    {
        string RabbitMqHostUrl { get; set; }
    }
}