using Ecom.Core.Entites;
using Ecom.Core.Interfaces;
using Ecom.Core.Serviecs;
using Ecom.infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.infrastructure.Repositries.Services
{
    
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork work;
        private readonly IConfiguration configuration;
        private readonly AppDbContext _context;

        public PaymentService(IUnitOfWork work,IConfiguration configuration,AppDbContext context)
        {
            this.work = work;
            this.configuration = configuration;
            _context = context;
        }
        public async Task<CustomerBasket> CreateOrderUpdatePaymentAsync(string basketID,int? deleveryMethodID)
        {
            var basket=await work.CustomerBasketRepositry.GetBasketAsync(basketID);

            StripeConfiguration.ApiKey = configuration["StripSetting:Secretkey"];

            decimal shippingPrice=0m;

            if (deleveryMethodID.HasValue)
            {
                var delivery = await _context.DeliveryMethods
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == deleveryMethodID.Value);

                shippingPrice = delivery.Price;
            }

            foreach (var item in basket.BasketItems)
            {
                var product = await work.ProductRepositry.GetByIdAsync(item.Id);
                item.Price = product.NewPrice;
            }

            PaymentIntentService paymentIntentService = new();

            PaymentIntent _intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentID))
            {
                var option = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(m => m.Quanatity * (m.Price * 100)) +(long) (shippingPrice * 100),
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card"}
                };
                _intent =await paymentIntentService.CreateAsync(option);

                basket.PaymentIntentID = _intent.Id;

                basket.ClientSecret= _intent.ClientSecret;
            }
            else
            {
                var option = new PaymentIntentUpdateOptions
                {
                    Amount = (long)basket.BasketItems.Sum(m => m.Quanatity * (m.Price * 100)) + (long)(shippingPrice * 100),
                };
                await paymentIntentService.UpdateAsync(basket.PaymentIntentID,option);
            }
            await work.CustomerBasketRepositry.UpdateBasketAsync(basket);
            return basket;  
        }
    }
}
