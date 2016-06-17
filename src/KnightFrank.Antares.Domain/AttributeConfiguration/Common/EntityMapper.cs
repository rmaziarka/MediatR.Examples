namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public class EntityMapper : IEntityMapper
    {
        public TEntity MapAllowedValues<TEntity, TKey, TSource>(TSource source, TEntity entity, IControlsConfiguration<TKey> config, PageType pageType,
            TKey key) where TKey : IStructuralEquatable, IStructuralComparable, IComparable
        {
            IList<InnerFieldState> fieldStates = config.GetInnerFieldsState(pageType, key, source);
            foreach (InnerFieldState field in fieldStates.Where(x => !x.Readonly && !x.Hidden))
            {
                var memberSelectorExpression = field.Expression.Body as MemberExpression;
                var property = memberSelectorExpression?.Member as PropertyInfo;
                if (property != null)
                {
                    PropertyInfo targetProperty = entity.GetType().GetProperty(property.Name);
                    targetProperty?.SetValue(entity, field.Compiled(source), null);
                }
            }

            return entity;
        }
    }
}
