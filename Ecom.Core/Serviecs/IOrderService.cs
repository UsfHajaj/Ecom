using Ecom.Core.DTOs;
using Ecom.Core.Entites.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Serviecs
{
    public interface IOrderService
    {
        Task<Orders> CreateOrdersAsync(OrderDTO orderDTO, string BuyerEmail);
        Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail);
        Task<OrderToReturnDTO> GetOrderByIdAsync(int Id, string BuyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync();
    }
}
