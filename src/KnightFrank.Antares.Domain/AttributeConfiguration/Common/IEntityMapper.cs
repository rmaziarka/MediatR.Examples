namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using System;
    using System.Collections;

    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;

    public interface IEntityMapper
    {
        TEntity MapAllowedValues<TEntity, TKey, TSource>(TSource source, TEntity entity, IControlsConfiguration<TKey> config,
            PageType pageType, TKey key) where TKey : IStructuralEquatable, IStructuralComparable, IComparable;
    }
}