using AutoMapper;
using Ecom.Api.Helper;
using Ecom.Core.DTOs;
using Ecom.Core.Entites.Products;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ecom.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        public CategoriesController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> Get()
        {
            try 
            {
                var category = await work.CategoryRepositry.GetAllAsync();
                if(category == null)
                {
                    return BadRequest(new ResponseAPI(400));
                }
                return Ok(category);
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
                var category = await work.CategoryRepositry.GetByIdAsync(id);
                if (category == null)
                {
                    return BadRequest(new ResponseAPI(400,$"Not Found Category Id={id}"));
                }
                return Ok(category);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("add-category")]
        public async Task<IActionResult> PostCategory(CategoryDTO categoryDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                var newCategory = mapper.Map<Category>(categoryDto);
                
                await work.CategoryRepositry.AddAsync(newCategory);
                return CreatedAtAction(nameof(PostCategory),new {id=newCategory.Id}, newCategory);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("update-category")]
        public async Task<IActionResult> PutCategory(UpdateCategoryDTO categoryDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest();
                var category = await work.CategoryRepositry.GetByIdAsync(categoryDto.Id);
                mapper.Map(categoryDto, category);
                
                await work.CategoryRepositry.UpdateAsync(category);
                return Ok(new ResponseAPI(200,$"Item Has Been Updated"));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("delete-caregory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await work.CategoryRepositry.GetByIdAsync(id);
                if (category == null) return NotFound();
                await work.CategoryRepositry.DeleteAsync(id);
                return Ok(new ResponseAPI(200, $"Item Has Been Deleted"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
