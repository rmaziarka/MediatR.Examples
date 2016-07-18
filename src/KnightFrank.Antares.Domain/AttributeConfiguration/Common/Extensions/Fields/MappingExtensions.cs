namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions.Fields
{
    using System;
    using System.Linq.Expressions;

    using FluentValidation.Internal;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public static class MappingExtensions
    {
        public static IField MapsInto<TEntity>(this IField field, Expression<Func<TEntity, object>> targetContainer)
        {
            field.InnerField.TargetContainerExpression = targetContainer;
            field.InnerField.TargetContainerCompiled = targetContainer.Compile().CoerceToNonGeneric();
            return field;
        }
    }
}
