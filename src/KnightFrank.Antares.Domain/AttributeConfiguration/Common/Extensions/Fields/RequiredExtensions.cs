namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions.Fields
{
    using System;
    using System.Collections.Generic;

    using FluentValidation;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public static class RequiredExtensions
    {
        public static Field<TEntity, TProperty> Required<TEntity, TProperty>(this Field<TEntity, TProperty> field)
        {
            field.InnerField.Required = true;
            field.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(field.Selector).NotEmpty().NotNull()));
            return field;
        }

        public static IList<Field<TEntity, TProperty>> Required<TEntity, TProperty>(this IList<Field<TEntity, TProperty>> fields)
        {
            foreach (Field<TEntity, TProperty> field in fields)
            {
                field.Required();
            }

            return fields;
        }
    }
}
