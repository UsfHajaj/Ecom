using AutoMapper;
using Ecom.Core.Interfaces;
using Ecom.Core.Serviecs;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositries;
using Ecom.infrastructure.Repositries.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure
{
    public static class infrastructureRegisteration
    {
        public static IServiceCollection infrastructureConfiguration(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));
            services.AddSingleton<IImageMangementService, ImageMangementService>();
            services.AddSingleton<IFileProvider>(
           new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            //Apply UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            

            services.AddDbContext<AppDbContext>(op => 
            {
                op.UseSqlServer(configuration.GetConnectionString("EcomDatabase"));
            }
            );
            return services;
        }
    }
}
