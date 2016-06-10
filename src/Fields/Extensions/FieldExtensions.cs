namespace Fields.Extensions
{
    using System;
    using System.Linq.Expressions;

    using FluentValidation;

    using Validators;

    public static class FieldExtensions
    {
        public static Field<TEntity,TProperty> GreaterThan<TEntity, TProperty>(this Field<TEntity,TProperty> field, TProperty limit) 
            where TProperty : IComparable, IComparable<TProperty>
        {
            field.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(field.Selector).GreaterThan(limit)));
            return field;
        }
    }
}
