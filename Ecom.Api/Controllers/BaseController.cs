using AutoMapper;
using Ecom.Core.Entites.Products;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IUnitOfWork work;
        protected readonly IMapper mapper;

        public BaseController(IUnitOfWork work,IMapper mapper)
        {
            this.work = work;
            this.mapper = mapper;
        }
    }
}
