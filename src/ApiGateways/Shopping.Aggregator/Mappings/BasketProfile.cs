using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Shopping.Aggregator.DTOs;

namespace Shopping.Aggregator.Mappings
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            // Create a map
            CreateMap<CatalogDto, BasketItemDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Summary, opt => opt.MapFrom(src => src.Summary))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ImageFile, opt => opt.MapFrom(src => src.ImageFile))
            .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}
