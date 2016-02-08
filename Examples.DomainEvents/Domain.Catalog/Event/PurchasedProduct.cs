using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Catalog.Event
{
    public class PurchasedProduct
    {
        public int ProductId { get; set; }

        public decimal Price { get; set; }
        
        public int Quantity { get; set; }
    }
}
