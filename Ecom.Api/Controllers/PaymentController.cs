using Ecom.Core.Entites;
using Ecom.Core.Serviecs;
using Ecom.infrastructure.Repositries.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<ActionResult<CustomerBasket>> Create(string busketId,int? deleveryMethodId)
        {
            return await _service.CreateOrderUpdatePaymentAsync(busketId, deleveryMethodId);
        }
    }
}
