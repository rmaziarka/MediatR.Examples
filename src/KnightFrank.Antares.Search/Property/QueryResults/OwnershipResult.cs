namespace KnightFrank.Antares.Search.Property.QueryResults
{
    using System;
    using System.Collections.Generic;

    public class OwnershipResult
    {
        public string Id { get; set; }

        public DateTime? SellDate { get; set; }

        public string OwnershipTypeId { get; set; }

        private EnumTypeItemResult OwnershipType { get; set; }

        public IList<ContactResult> Contacts { get; set; }
    }
}
