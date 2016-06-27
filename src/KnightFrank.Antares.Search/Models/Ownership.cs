namespace KnightFrank.Antares.Search.Models
{
    using System;
    using System.Collections.Generic;

    public class Ownership
    {
        public string Id { get; set; }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? SellDate { get; set; }

        public string PropertyId { get; set; }

        public decimal? BuyPrice { get; set; }

        public decimal? SellPrice { get; set; }

        public string OwnershipTypeId { get; set; }

        public EnumTypeItem OwnershipType { get; set; }

        public IList<OwnershipContact> OwnershipContacts { get; set; }
    }
}
