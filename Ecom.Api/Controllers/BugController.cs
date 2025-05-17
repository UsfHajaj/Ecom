using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugController : BaseController
    {
        public BugController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("not-found")]
        public async Task<IActionResult> GetNotFound()
        {
            var category =await work.CategoryRepositry.GetByIdAsync(100);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpGet("server-error")]
        public async Task<IActionResult> GetServerError()
        {
            var category = await work.CategoryRepositry.GetByIdAsync(100);
            category.Name = "";
            return Ok(category);
        }
        [HttpGet("bad-request/{id}")]
        public async Task<IActionResult> GetBadRequest(int id )
        {
            return Ok();
        }
        [HttpGet("bad-request/")]
        public async Task<IActionResult> GetBadRequest()
        {
            return BadRequest();
        } 
    }
}
