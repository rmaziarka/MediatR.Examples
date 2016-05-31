namespace KnightFrank.Antares.Domain.LatestView.DataProviders
{
    using KnightFrank.Antares.Domain.LatestView.QueryResults;

    public interface ILatestViewDataProvider
    {
        /// <summary>
        /// Gets the latest view data the currently handled entity type.
        /// </summary>
        /// <param name="entityLatestViews">The x.</param>
        /// <returns></returns>
        LatestViewQueryResultItem GetData(EntityLatestViews entityLatestViews);
    }
}