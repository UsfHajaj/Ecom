using AutoMapper;
using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Ecom.Core.Serviecs;
using Ecom.infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IImageMangementService _imageMangementService;
        private readonly IConnectionMultiplexer redis;
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken token;

        public ICategoryRepositry CategoryRepositry { get; }

        public IPhotoRepositry PhotoRepositry { get; }

        public IProductRepositry ProductRepositry { get; }

        public ICustomerBasketRepositry CustomerBasketRepositry { get; }

        public IAuth Auth { get; }

        public UnitOfWork(AppDbContext context
            , IMapper mapper
            , IImageMangementService imageMangementService
            , IConnectionMultiplexer redis,
UserManager<AppUser> userManager,
IEmailService emailService,
SignInManager<AppUser> signInManager,
IGenerateToken token)
        {
            _context = context;
            _mapper = mapper;
            _imageMangementService = imageMangementService;
            this.redis = redis;
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.token = token;

            CategoryRepositry = new CategoryRepositry(_context);

            PhotoRepositry = new PhotoRepositry(_context);

            ProductRepositry = new ProductRepositry(_context, _mapper, _imageMangementService);

            CustomerBasketRepositry = new CustomerBasketRepositry(redis);

            Auth = new AuthRepositry(userManager, emailService, signInManager,token,context);
            
        }
    }
}
