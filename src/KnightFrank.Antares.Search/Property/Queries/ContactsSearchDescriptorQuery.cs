namespace KnightFrank.Antares.Search.Property.Queries
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Search.Common.Queries;

    public class ContactsSearchDescriptorQuery : ISearchDescriptorQuery
    {
        public IList<string> ContactIds { get; set; }
    }
}
