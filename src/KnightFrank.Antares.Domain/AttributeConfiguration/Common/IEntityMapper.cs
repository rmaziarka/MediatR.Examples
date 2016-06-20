namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    public interface IEntityMapper<TEntity>
    {
        TEntity MapAllowedValues<TSource>(TSource source, TEntity entity, PageType pageType);

        TEntity NullifyDisallowedValues(TEntity entity, PageType pageType);
    }
}