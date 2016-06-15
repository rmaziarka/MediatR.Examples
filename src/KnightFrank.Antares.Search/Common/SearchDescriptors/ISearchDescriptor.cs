namespace KnightFrank.Antares.Search.Common.SearchDescriptors
{
    using KnightFrank.Antares.Search.Common.Queries;

    using Nest;

    public interface ISearchDescriptor<TM, TQ>
        where TM : class where TQ : ISearchDescriptorQuery
    {
        SearchDescriptor<TM> Create(TQ query);
    }
}
