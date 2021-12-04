using AutoMapper;
using Basket.API.DTOs;
using EventBus.Messages.Events;

namespace Basket.API.Profiles
{
    public class BasketCheckoutProfile : Profile
    {
        public BasketCheckoutProfile()
        {
            CreateMap<BasketCheckoutDto, BasketCheckoutEvent>();
        }
    }
}