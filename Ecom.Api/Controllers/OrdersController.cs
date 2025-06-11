using Ecom.Core.DTOs;
using Ecom.Core.Serviecs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Icao;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost("create-order")]
        public async Task<IActionResult> Create(OrderDTO orderDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            var order = await _orderService.CreateOrdersAsync(orderDTO,email);

            return Ok(order);
            
        }
        [HttpGet("get-orders-for-user")]
        public async Task<IActionResult> GetUserOrder()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var order=await _orderService.GetAllOrdersForUserAsync(email);

            return Ok(order);
        }
        [HttpGet("get-order-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var email=User.FindFirst(ClaimTypes.Email)?.Value;
            var order=await _orderService.GetOrderByIdAsync(id,email);
            return Ok(order);
        }
        [HttpGet("get-delivery")]
        public async Task<IActionResult> GetDeliveryMethond()
        {
            var delivery = await _orderService.GetDeliveryMethodAsync();
            return Ok(delivery);
        }
    }
}
