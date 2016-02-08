using System.Collections.Generic;

namespace Domain.Catalog.Dto
{
    public class NewPurchaseDto
    {
        public int UserId { get; set; }

        public IEnumerable<PurchasedProductDto> Products { get; set; }
    }
}
