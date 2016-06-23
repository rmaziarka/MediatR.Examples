namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions.Fields
{
    using System.Collections.Generic;

    using FluentValidation;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public static class LengthExtension
    {
        public static Field<TEntity, string> Length<TEntity>(this Field<TEntity, string> field, int maxLength)
        {
            field.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(field.Selector).Length(maxLength)));
            return field;
        }

        public static Field<TEntity, string> Length<TEntity>(this Field<TEntity, string> field, int minLength, int maxLength)
        {
            field.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(field.Selector).Length(minLength, maxLength)));
            return field;
        }

        public static IList<Field<TEntity, string>> Length<TEntity>(this IList<Field<TEntity, string>> fields, int maxLength)
        {
            foreach (Field<TEntity, string> field in fields)
            {
                field.Length(maxLength);
            }

            return fields;
        }

        public static IList<Field<TEntity, string>> Length<TEntity>(this IList<Field<TEntity, string>> fields, int minLength, int maxLength)
        {
            foreach (Field<TEntity, string> field in fields)
            {
                field.Length(minLength, maxLength);
            }

            return fields;
        }
    }
}
