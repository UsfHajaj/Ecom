using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entites.Order;

namespace Ecom.Api.Mapping
{
    public class OrderMapping : Profile
    {
        public OrderMapping()
        {
            CreateMap<ShippingAddress, ShipAddressDTO>().ReverseMap();
        }
    }
}
