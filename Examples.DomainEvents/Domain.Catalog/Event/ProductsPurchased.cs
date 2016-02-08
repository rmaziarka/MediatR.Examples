using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Event;

namespace Domain.Catalog.Event
{
    public class ProductsPurchased:DomainEvent
    {
        public int UserId { get; set; }

        public decimal UserDiscount { get; set; }

        public IEnumerable<PurchasedProduct> PurchasedProducts { get; set; } 
    }
}
