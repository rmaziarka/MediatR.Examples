namespace KnightFrank.Antares.Search.Property.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Search.Common.QueryResults;
    using KnightFrank.Antares.Search.Common.SearchDescriptors;
    using KnightFrank.Antares.Search.Models;
    using KnightFrank.Antares.Search.Property.Queries;
    using KnightFrank.Antares.Search.Property.QueryResults;

    using MediatR;

    using Nest;

    public class PropertiesPageableQueryHandler : IRequestHandler<PropertiesPageableQuery, PageableResult<PropertyResult>>
    {
        private readonly ISearchDescriptor<Contact, ContactsSearchDescriptorQuery> contactSearchDescriptor;

        private readonly ISearchDescriptor<Property, PropertiesPageableQuery> propertySearchDescriptor;

        private readonly IElasticClient elasticClient;

        public PropertiesPageableQueryHandler(
            ISearchDescriptor<Property, PropertiesPageableQuery> propertySearchDescriptor,
            ISearchDescriptor<Contact, ContactsSearchDescriptorQuery> contactSearchDescriptor,
            IElasticClient elasticClient)
        {
            this.propertySearchDescriptor = propertySearchDescriptor;
            this.contactSearchDescriptor = contactSearchDescriptor;
            this.elasticClient = elasticClient;
        }

        public PageableResult<PropertyResult> Handle(PropertiesPageableQuery pageableQuery)
        {
            ISearchResponse<Property> propertySearchResponse = this.FindProperties(pageableQuery);
            List<Property> properties = propertySearchResponse.Documents.ToList();
            IList<string> contactIds = this.ExtractContactIds(properties);
            List<Contact> contacts = this.FindContacts(contactIds);

            List<PropertyResult> propertyResults = properties.Select(p => Mapper.Map<PropertyResult>(p)).ToList();

            this.FillOwnershipContactData(propertyResults, properties, contacts);

            return new PageableResult<PropertyResult>
                       {
                           Data = propertyResults,
                           Total = propertySearchResponse.Total,
                           Page = pageableQuery.Page,
                           Size = pageableQuery.Size
                       };
        }

        private ISearchResponse<Property> FindProperties(PropertiesPageableQuery pageableQuery)
        {
            SearchDescriptor<Property> propertySearchDescriptor =
                this.propertySearchDescriptor.Create(pageableQuery)
                    .Take(pageableQuery.Size)
                    .Skip(pageableQuery.Page * pageableQuery.Size);

            return this.elasticClient.Search<Property>(propertySearchDescriptor);
        }

        private List<Contact> FindContacts(IList<string> contactIds)
        {
            if (contactIds == null || contactIds.Count == 0)
            {
                return new List<Contact>();
            }
            var searchContactQuery = new ContactsSearchDescriptorQuery { ContactIds = contactIds };
            SearchDescriptor<Contact> contactSearchDescriptor = this.contactSearchDescriptor.Create(searchContactQuery);
            return this.elasticClient.Search<Contact>(contactSearchDescriptor).Documents.ToList();
        }

        private List<string> ExtractContactIds(List<Property> properties)
        {
            return
                properties.Where(p => p.Ownerships != null)
                          .SelectMany(p => p.Ownerships)
                          .Where(o => o.OwnershipContacts != null)
                          .SelectMany(o => o.OwnershipContacts)
                          .Where(oc => oc.ContactId != null)
                          .Select(oc => oc.ContactId)
                          .ToList();
        }

        private void FillOwnershipContactData(
            List<PropertyResult> propertyResults,
            List<Property> properties,
            List<Contact> contacts)
        {
            propertyResults.ForEach(p => this.FillOwnershipContactData(p, properties.Single(x => x.Id == p.Id), contacts));
        }

        private void FillOwnershipContactData(PropertyResult propertyResult, Property property, List<Contact> contacts)
        {
            propertyResult.Ownerships?.ToList().ForEach(
                o =>
                    {
                        IEnumerable<string> contactIds =
                            property.Ownerships.Single(x => x.Id == o.Id).OwnershipContacts?.Select(c => c.ContactId);
                        o.Contacts =
                            contacts.Where(c => contactIds.Contains(c.Id)).Select(c => Mapper.Map<ContactResult>(c)).ToList();
                    });
        }
    }
}
