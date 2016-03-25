namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Contact;
    using KnightFrank.Antares.Dal.Model.Enum;

    public class Ownership : BaseEntity
    {
        public Ownership()
        {
            this.Contacts = new List<Contact>();
        }

        public DateTime? PurchaseDate { get; set; }

        public DateTime? SellDate { get; set; }

        public ICollection<Contact> Contacts { get; set; }

        public virtual Property Property { get; set; }

        public Guid PropertyId { get; set; }

        public decimal? BuyPrice { get; set; }

        public decimal? SellPrice { get; set; }

        public bool CurrentOwner { get; set; }

        public Guid OwnershipTypeId { get; set; }

        public EnumTypeItem OwnershipType { get; set; }
    }
}
