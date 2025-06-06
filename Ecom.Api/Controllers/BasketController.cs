using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : BaseController
    {
        public BasketController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult> GetBasketItem(string id)
        {
            var result=await work.CustomerBasketRepositry.GetBasketAsync(id);
            if (result is null)
            {
                return Ok(new CustomerBasket());
            }
            return Ok(result);
        }
        [HttpPost("update-basket")]
        public async Task<IActionResult> AddBasket(CustomerBasket customerBasket)
        {
            var result=await work.CustomerBasketRepositry.UpdateBasketAsync(customerBasket);
            return Ok(result);
        }
        [HttpDelete("delete-basket/{id}")]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            var result = await work.CustomerBasketRepositry.DeleteBasketAsync(id);
            return result?Ok(new ResponseAPI(200,"Deleted Successfull"))
                :BadRequest(new ResponseAPI(400));
        }
    }
}
