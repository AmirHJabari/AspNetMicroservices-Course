using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.API.Profiles
{
    public class CheckoutOrderProfile : Profile
    {
        public CheckoutOrderProfile()
        {
            CreateMap<BasketCheckoutEvent, CheckoutOrderCommand>();
        }
    }
}