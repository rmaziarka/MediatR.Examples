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

    public abstract class BaseEntityMapper<TEntity, TKey> : IEntityMapper<TEntity> where TKey : IStructuralEquatable, IStructuralComparable, IComparable
    {
        public TEntity MapAllowedValues<TSource>(TSource source, TEntity entity, IControlsConfiguration<TKey> config, PageType pageType, TKey key)
        {
            IList<InnerFieldState> fieldStates = config.GetInnerFieldsState(pageType, key, source);
            IEnumerable<InnerFieldState> fieldsToUpdate = fieldStates.Where(x => !x.Readonly);
            foreach (InnerFieldState field in fieldsToUpdate)
            {
                this.SetFieldValue(entity, field, field.Compiled(source));
            }

            return entity;
        }

        public TEntity NullifyDisallowedValues(TEntity entity, IControlsConfiguration<TKey> config, PageType pageType, TKey key)
        {
            IList<InnerFieldState> fieldStates = config.GetInnerFieldsState(pageType, key, entity);
            Func<InnerFieldState, bool> noOtherActiveFieldIsDefinedForTheSameEntityExpression =
                x => !fieldStates.Any(s => !s.Hidden && s.Expression.ToString() == x.Expression.ToString());

            IEnumerable<InnerFieldState> fieldsToNullify =
                fieldStates.Where(x => x.Hidden && noOtherActiveFieldIsDefinedForTheSameEntityExpression(x));

            foreach (InnerFieldState field in fieldsToNullify)
            {
                object defaultValue = field.PropertyType.IsValueType ? Activator.CreateInstance(field.PropertyType) : null;
                this.SetFieldValue(entity, field, defaultValue);
            }

            return entity;
        }

        public abstract TEntity MapAllowedValues<TSource>(TSource source, TEntity entity, PageType pageType);

        public abstract TEntity NullifyDisallowedValues(TEntity entity, PageType pageType);

        private void SetFieldValue<T>(T entity, InnerFieldState field, object value)
        {
            var memberSelectorExpression = field.Expression.Body as MemberExpression;
            var property = memberSelectorExpression?.Member as PropertyInfo;
            if (property != null)
            {
                PropertyInfo targetProperty = entity.GetType().GetProperty(property.Name);
                targetProperty?.SetValue(entity, value);
            }
        }
    }
}
