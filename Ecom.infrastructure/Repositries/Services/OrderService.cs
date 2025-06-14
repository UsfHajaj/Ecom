using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entites.Order;
using Ecom.Core.Interfaces;
using Ecom.Core.Serviecs;
using Ecom.infrastructure.Data;
using Microsoft.AspNetCore.WebUtilities;
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
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork work,AppDbContext context,IMapper mapper,IPaymentService paymentService)
        {
            _work = work;
            _context = context;
            _mapper = mapper;
            _paymentService = paymentService;
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

            var existOrder=await _context.Orders.Where(m=>m.PaymentIntentId==basket.PaymentIntentID).FirstOrDefaultAsync();

            if (existOrder is not null)
            {
                _context.Orders.Remove(existOrder);

                await _paymentService.CreateOrderUpdatePaymentAsync(basket.PaymentIntentID,deliveryMethod.Id);
            }

            var order = new Orders(BuyerEmail, subTotal, shiping, deliveryMethod, orderItems,basket.PaymentIntentID);

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            await _work.CustomerBasketRepositry.DeleteBasketAsync(orderDTO.basketId);
            return order;
        }

        public async Task<IReadOnlyList<OrderToReturnDTO>> GetAllOrdersForUserAsync(string BuyerEmail)
        {
            var orders = await _context.Orders.Where(m => m.BuyerEmail == BuyerEmail)
                .Include(inc => inc.orderItems).Include(inc => inc.deliveryMethod)
                .ToListAsync();
            var result = _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);
            
            return result;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            return await _context.DeliveryMethods.AsNoTracking().ToListAsync();
        }

        public async Task<OrderToReturnDTO> GetOrderByIdAsync(int Id, string BuyerEmail)
        {
            var order = await _context.Orders
                .Where(m => m.Id == Id && m.BuyerEmail == BuyerEmail)
                .Include(m=>m.orderItems)
                .Include(m=>m.deliveryMethod)
                .FirstOrDefaultAsync();
            var result = _mapper.Map<OrderToReturnDTO>(order);
            return result;
        }
    }
}
