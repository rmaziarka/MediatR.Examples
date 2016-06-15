namespace KnightFrank.Antares.Search.Property.QueryHandlers
{
    using KnightFrank.Antares.Search.Common.SearchDescriptors;
    using KnightFrank.Antares.Search.Models;
    using KnightFrank.Antares.Search.Property.Queries;

    using Nest;

    internal class PropertiesSearchDescriptor : ISearchDescriptor<Property, PropertiesPageableQuery>
    {
        public SearchDescriptor<Property> Create(PropertiesPageableQuery pageableQuery)
        {
            SearchDescriptor<Property> searchDescriptor =
                new SearchDescriptor<Property>().Query(
                    q =>
                    q.MultiMatch(
                        mm =>
                        mm.Query(pageableQuery.Query)
                          .Fields(
                              mmf =>
                              mmf.Fields(
                                  f => f.Address.PropertyNumber,
                                  f => f.Address.PropertyName,
                                  f => f.Address.Line1,
                                  f => f.Address.Line2,
                                  f => f.Address.Line3,
                                  f => f.Address.Postcode,
                                  f => f.Address.City,
                                  f => f.Address.County))
                          .Type(TextQueryType.CrossFields)));

            return searchDescriptor;
        }
    }
}
