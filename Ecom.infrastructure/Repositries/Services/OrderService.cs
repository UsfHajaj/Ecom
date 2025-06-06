using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entites.Order;
using Ecom.Core.Interfaces;
using Ecom.Core.Serviecs;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _work;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork work,AppDbContext context,IMapper mapper)
        {
            _work = work;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Orders> CreateOrdersAsync(OrderDTO orderDTO, string BuyerEmail)
        {
            var basket = await _work.CustomerBasketRepositry.GetBasketAsync(orderDTO.basketId);

            List<OrderItem> orderItems= new List<OrderItem>();
            foreach(var item in basket.BasketItems)
            {
                var product = await _work.ProductRepositry.GetByIdAsync(item.Id);
                var orderItem = new OrderItem(product.Id,
                    item.Image
                    ,product.Name
                    ,item.Price,
                    item.Quanatity);
                orderItems.Add(orderItem);
            }
            var deliveryMethod=await _context.DeliveryMethods
                .FirstOrDefaultAsync(m=>m.Id==orderDTO.deliveryMethodId);
            var subTotal = orderItems.Sum(m => m.Price * m.Quntity);

            var shiping = _mapper.Map<ShippingAddress>( orderDTO.shipAddress);
            var order = new Orders(BuyerEmail, subTotal, shiping, deliveryMethod, orderItems);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public Task<IReadOnlyList<Orders>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Orders> GetOrderByIdAsync(int Id, string BuyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
