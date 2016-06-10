namespace Fields.Extensions
{
    using System;
    using System.Collections.Generic;
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

        public static IList<Control> IsHiddenWhen<TEntity>(this IList<Control> controls, Expression<Func<TEntity, bool>> expression)
        {
            foreach (Control control in controls)
            {
                control.SetHiddenRule(expression);
            }

            return controls;
        }

        public static IList<Control> IsReadonlyWhen<TEntity>(this IList<Control> controls, Expression<Func<TEntity, bool>> expression)
        {
            foreach (Control control in controls)
            {
                control.SetReadonlyRule(expression);
            }

            return controls;
        }
    }
}
