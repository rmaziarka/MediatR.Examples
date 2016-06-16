namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public static class EntityExtensions
    {
        public static void MapAllowedValues<TEntity, TKey1, TKey2, TSource>(this TSource source, TEntity entity, IControlsConfiguration<TKey1, TKey2> config, PageType pageType, TKey1 key1, TKey2 key2)
        {
            IList<InnerFieldState> fieldStates = config.GetInnerFieldsState(pageType, key1, key2, source);
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
            
        }
    }
}
