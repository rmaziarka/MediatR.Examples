namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    public interface IEntityMapper
    {
        TEntity MapAllowedValues<TEntity, TKey1, TKey2, TSource>(TSource source, TEntity entity, IControlsConfiguration<TKey1, TKey2> config, PageType pageType, TKey1 key1, TKey2 key2);
    }
}