namespace KnightFrank.Antares.Search.Property.QueryHandlers
{
    using KnightFrank.Antares.Search.Common.SearchDescriptors;
    using KnightFrank.Antares.Search.Models;
    using KnightFrank.Antares.Search.Property.Queries;

    using Nest;

    internal class ContactsSearchDescriptor : ISearchDescriptor<Contact, ContactsSearchDescriptorQuery>
    {
        public SearchDescriptor<Contact> Create(ContactsSearchDescriptorQuery pageableQuery)
        {
            SearchDescriptor<Contact> searchDescriptor =
                new SearchDescriptor<Contact>().Query(
                    q => q.Ids(ids => ids.Types(Types.Parse("contact")).Values(pageableQuery.ContactIds)));

            return searchDescriptor;
        }
    }
}
