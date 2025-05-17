using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entites.Products;
using Ecom.Core.Interfaces;
using Ecom.Core.Serviecs;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries
{
    public class ProductRepositry : GenericRepositry<Product>, IProductRepositry
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageMangementService _imageMangementService;

        public ProductRepositry(AppDbContext context,IMapper mapper,IImageMangementService imageMangementService) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _imageMangementService = imageMangementService;
        }

        public async Task<bool> AddAsync(CreateProductDTO productDTO)
        {
            if (productDTO == null) return false;
            var product= _mapper.Map<Product>(productDTO);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            var ImagePath=await _imageMangementService.AddImageAsync(productDTO.Photo,productDTO.Name);

            var photo = ImagePath.Select(m => new Photo
            {
                ImageName = m,
                ProductId = product.Id,

            }).ToList();
            await _context.Photos.AddRangeAsync(photo);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> UpdateAsync(UpdateProductDTO productDTO)
        {
            if (productDTO == null) return false;


            var product = await _context.Products
                .Include(m => m.Category)
                .Include(m => m.Photos)
                .FirstOrDefaultAsync(m=>m.Id==productDTO.Id);

            if (product == null) return false;

            _mapper.Map(productDTO,product);


            var FindPhoto = await _context.Photos.Where(m => m.ProductId == productDTO.Id)
                .ToListAsync();

            foreach (var item in FindPhoto)
            {
                _imageMangementService.DeleteImageAsync(item.ImageName);
            }

            _context.Photos.RemoveRange(FindPhoto);


            var ImagePath=await _imageMangementService.AddImageAsync(productDTO.Photo, productDTO.Name);
            var photo = ImagePath.Select(m => new Photo
            {
                ImageName = m,
                ProductId = product.Id,
            }).ToList();
            await _context.Photos.AddRangeAsync(photo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(Product product)
        {

            var FindPhoto = _context.Photos.Where(m => m.ProductId == product.Id)
                .ToList();

            foreach (var item in FindPhoto)
            {
                _imageMangementService.DeleteImageAsync(item.ImageName);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
