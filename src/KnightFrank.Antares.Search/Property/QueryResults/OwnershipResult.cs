namespace KnightFrank.Antares.Search.Property.QueryResults
{
    using System.Collections.Generic;

    public class OwnershipResult
    {
        public string Id { get; set; }

        public string OwnershipTypeId { get; set; }

        public IList<ContactResult> Contacts { get; set; }
    }
}
