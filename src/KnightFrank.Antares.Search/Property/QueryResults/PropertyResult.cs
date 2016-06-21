namespace KnightFrank.Antares.Search.Property.QueryResults
{
    using System.Collections.Generic;

    public class PropertyResult
    {
        public string Id { get; set; }

        public string AddressId { get; set; }

        public AddressResult Address { get; set; }

        public IList<OwnershipResult> Ownerships { get; set; }
    }
}
