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
                MapField(entity, field, field.Compiled(source));
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
                SetFieldValue(entity, field, defaultValue);
            }

            return entity;
        }

        public abstract TEntity MapAllowedValues<TSource>(TSource source, TEntity entity, PageType pageType);

        public abstract TEntity NullifyDisallowedValues(TEntity entity, PageType pageType);

        private static void SetFieldValue<T>(T entity, InnerFieldState field, object value)
        {
            string propertyPath = GetFullPropertyPathName(field.Expression);
            SetNestedPropertyValue(propertyPath, entity, value);
        }

        private static void MapField<T>(T entity, InnerFieldState field, object value)
        {
            string fullPropertyName = GetFullPropertyPathName(field.Expression);
            object targetEntity = entity;
            if (field.TargetContainer != null)
            {
                targetEntity = field.TargetContainerCompiled(entity);
            }

            SetNestedPropertyValue(fullPropertyName, targetEntity, value);
        }

        private static void SetNestedPropertyValue(string propertyFullName, object entity, object value)
        {
            List<string> tokens = propertyFullName.Split('.').ToList();
            if (tokens.Count == 1)
            {
                PropertyInfo targetProperty = entity.GetType().GetProperty(propertyFullName);
                if (targetProperty?.PropertyType.IsInstanceOfType(value) == true)
                {
                    targetProperty.SetValue(entity, value);
                }
            }
            else
            {
                string containingObjectFullPropertyName = string.Join(".", tokens.Take(tokens.Count - 1));
                object containingObject = GetNestedPropertyValue(containingObjectFullPropertyName, entity);
                string targetPropertyName = tokens.Last();
                PropertyInfo targetProperty = containingObject?.GetType().GetProperty(targetPropertyName);
                if (targetProperty?.PropertyType.IsInstanceOfType(value) == true)
                {
                    targetProperty.SetValue(containingObject, value);
                }
            }
        }

        private static string GetFullPropertyPathName(LambdaExpression expression)
        {
            return expression.Body.ToString().Replace(expression.Parameters[0] + ".", string.Empty);
        }

        private static object GetNestedPropertyValue(string propertyFullName, object entity)
        {
            if (entity == null)
            {
                return null;
            }

            foreach (string propertyNamePart in propertyFullName.Split('.'))
            {
                Type entityType = entity.GetType();
                PropertyInfo info = entityType.GetProperty(propertyNamePart);
                if (info == null)
                {
                    return null;
                }

                entity = info.GetValue(entity, null);
            }

            return entity;
        }
    }
}
