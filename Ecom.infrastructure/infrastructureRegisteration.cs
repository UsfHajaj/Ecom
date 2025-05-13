using AutoMapper;
using Ecom.Core.Interfaces;
using Ecom.infrastructure.Data;
using Ecom.infrastructure.Repositries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            //Apply UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<AppDbContext>(op => 
            {
                op.UseSqlServer(configuration.GetConnectionString("EcomDatabase"));
            }
            );
            return services;
        }
    }
}
