using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entites
{
    public class CustomerBasket
    {
        public CustomerBasket() { }

        public CustomerBasket(string id) 
        {
            Id = id;
        }
        public string Id { get; set; }//key
        public string? PaymentIntentID { get; set; }
        public string? ClientSecret { get; set; }
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();//value
    }
}
