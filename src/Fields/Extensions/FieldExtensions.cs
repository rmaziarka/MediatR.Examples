namespace Fields.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentValidation;

    using Validators;

    public static class FieldExtensions
    {
        public static Field<TEntity, TProperty> GreaterThan<TEntity, TProperty>(this Field<TEntity, TProperty> field, TProperty limit)
            where TProperty : IComparable, IComparable<TProperty>
        {
            field.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(field.Selector).GreaterThan(limit)));
            return field;
        }

        public static IList<Control> IsHiddenWhen<TEntity>(this IList<Control> controls, Expression<Func<TEntity, bool>> expression)
        {
            return SetControlExpression(controls, expression, false);
        }

        public static IList<Control> IsReadonlyWhen<TEntity>(this IList<Control> controls, Expression<Func<TEntity, bool>> expression)
        {
            return SetControlExpression(controls, expression, true);
        }

        private static IList<Control> SetControlExpression<TEntity>(IList<Control> controls, Expression<Func<TEntity, bool>> expression, bool readonlyExpression)
        {
            if (controls.Count > 1)
            {
                // epxression is stronly typed so it cannot be set for multiple controls but only for one control from specyfic mode.
                throw new NotSupportedException();
            }

            foreach (Control control in controls)
            {
                if (readonlyExpression)
                {
                    control.SetReadonlyRule(expression);
                }
                else
                {
                    control.SetHiddenRule(expression);
                }
            }

            return controls;
        }

        public static IList<Control> FieldIsReadonlyWhen<TEntity>(this IList<Control> controls, Expression<Func<TEntity, object>> field, Expression<Func<TEntity, bool>> expression)
        {
            return SetFieldExpression(controls, field, expression, true);
        }

        public static IList<Control> FieldIsHiddenWhen<TEntity>(this IList<Control> controls, Expression<Func<TEntity, object>> field, Expression<Func<TEntity, bool>> expression)
        {
            return SetFieldExpression(controls, field, expression, false);
        }

        private static IList<Control> SetFieldExpression<TEntity>(IList<Control> controls, Expression<Func<TEntity, object>> field, Expression<Func<TEntity, bool>> expression, bool readonlyExpression)
        {
            if (controls.Count > 1)
            {
                // epxression is stronly typed so it cannot be set for multiple controls but only for one control from specyfic mode.
                throw new NotSupportedException();
            }

            if (controls.Count == 1)
            {
                Control control = controls.First();
                if (readonlyExpression)
                {
                    control.SetFieldReadonlyRule(field, expression);
                }
                else
                {
                    control.SetFieldReadonlyRule(field, expression);
                }
            }

            return controls;
        }

    }
}
