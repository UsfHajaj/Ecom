using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entites;
using Ecom.Core.Entites.Order;

namespace Ecom.Api.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<Orders, OrderToReturnDTO>()
                .ForMember(dis=>dis.deliveryMethod,
                o=>o.MapFrom(op=>op.deliveryMethod.Name))
                .ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<ShippingAddress, ShipAddressDTO>().ReverseMap();

            CreateMap<ShipAddressDTO, Address>().ReverseMap();
        }
    }
}
