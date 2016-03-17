namespace KnightFrank.Antares.Dal.Model
{
    using System;
    using System.Collections.Generic;

    public class Ownership : BaseEntity
    {
        public Ownership()
        {
            this.Contacts = new List<Contact>();
        }

        public DateTime? PurchaseDate { get; set; }
        public DateTime? SellDate { get; set; }

        public List<Contact> Contacts { get; set; }

        public virtual Property Property { get; set; }
        public Guid PropertyId { get; set; }

        public decimal? BuyPrice { get; set; }
        public decimal? SellPrice { get; set; }

        public bool CurrentOwner { get; set; }

        public Guid OwnershipTypeId { get; set; }
        public EnumTypeItem OwnershipType { get; set; }
    }
}