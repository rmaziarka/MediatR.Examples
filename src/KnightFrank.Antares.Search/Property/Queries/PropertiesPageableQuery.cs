namespace KnightFrank.Antares.Search.Property.Queries
{
    using KnightFrank.Antares.Search.Common.Queries;
    using KnightFrank.Antares.Search.Common.QueryResults;
    using KnightFrank.Antares.Search.Property.QueryResults;

    public class PropertiesPageableQuery : BasePageableQuery<PageableResult<PropertyResult>>, ISearchDescriptorQuery
    {
        public string Query { get; set; }
    }
}
