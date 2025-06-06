using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.DTOs;
using Ecom.Core.Entites.Products;
using Ecom.Core.Interfaces;
using Ecom.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        public ProductsController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }
        [HttpGet("get-all")]
        public async Task<IActionResult> get([FromQuery] ProductParams? productParams)
        {
            try
            {
                var product = await work.ProductRepositry
                    .GetAllAsync(productParams);

                //var totalCount = await work.ProductRepositry.CountAsync();
                return Ok(new Pagination<ProductDTO>(productParams.PageNumber,productParams.PageSize, product.TotalCount, product.Products));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var product = await work.ProductRepositry
                    .GetByIdAsync(id,x => x.Category, x => x.Photos);
                var result=mapper.Map<ProductDTO>(product);
                if (product == null) return BadRequest(new ResponseAPI(400, "Item Not Found"));
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("add-product")]
        public async Task<IActionResult> PostProduct(CreateProductDTO ProductDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseAPI(400));
                //var product = mapper.Map<Product>(ProductDto);
                await work.ProductRepositry.AddAsync(ProductDto); 
                return Ok(new ResponseAPI(200, "Item Has Been Created"));
            }
            catch(Exception ex)
            {
                return BadRequest(new ResponseAPI(400,ex.Message));
            }
        }
        [HttpPut("update-product")]
        public async Task<IActionResult> PutProduct(UpdateProductDTO productDto)
        {
            try
            {
                var product = await work.ProductRepositry.GetByIdAsync(productDto.Id);
                if (product == null) return BadRequest(new ResponseAPI(400, "Item Not Found"));
                await work.ProductRepositry.UpdateAsync(productDto);
                return Ok(new ResponseAPI(200, "Item Has Been Updated"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await work.ProductRepositry.GetByIdAsync(id);
                if (product == null) return BadRequest(new ResponseAPI(400, "Item Not Found"));
                await work.ProductRepositry.DeleteAsync(product);
                return Ok(new ResponseAPI(200, "Item Has Been Deleted"));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
