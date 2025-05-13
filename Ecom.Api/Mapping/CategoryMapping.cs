using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entites.Products;

namespace Ecom.Api.Mapping
{
    public class CategoryMapping:Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDTO, Category>().ReverseMap();
            CreateMap<UpdateCategoryDTO, Category>().ReverseMap();
        }
    }
}
