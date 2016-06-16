namespace KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using FluentValidation;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Fields;

    public static class FieldExtensions
    {
        public static Field<TEntity, TProperty> GreaterThan<TEntity, TProperty>(this Field<TEntity, TProperty> field, TProperty limit)
            where TProperty : IComparable, IComparable<TProperty>
        {
            field.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(field.Selector).GreaterThan(limit)));
            return field;
        }

        public static Field<TEntity, TProperty> Required<TEntity, TProperty>(this Field<TEntity, TProperty> field)
        {
            field.InnerField.Required = true;
            field.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(field.Selector).NotEmpty().NotNull()));
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

        public static IList<Control> FieldIsReadonlyWhen<TEntity, TProperty>(this IList<Control> controls, Expression<Func<TEntity, TProperty>> field, Expression<Func<TEntity, bool>> expression)
        {
            return SetFieldExpression(controls, field, expression, true);
        }

        public static IList<Control> FieldIsHiddenWhen<TEntity, TProperty>(this IList<Control> controls, Expression<Func<TEntity, TProperty>> field, Expression<Func<TEntity, bool>> expression)
        {
            return SetFieldExpression(controls, field, expression, false);
        }

        public static IList<Control> FieldHasAllowed<TEntity, TProperty, TEnum>(this IList<Control> controls, Expression<Func<TEntity, TProperty>> field, IEnumerable<TEnum> allowedValues) where TEnum:struct
        {
            return SetAllowedValues(controls, field, allowedValues);
;        }

        private static IList<Control> SetFieldExpression<TEntity, TProperty>(IList<Control> controls, Expression<Func<TEntity, TProperty>> field, Expression<Func<TEntity, bool>> expression, bool readonlyExpression)
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
                    control.SetFieldHiddenRule(field, expression);
                }
            }

            return controls;
        }

        private static IList<Control> SetAllowedValues<TEntity, TProperty, TEnum>(IList<Control> controls, Expression<Func<TEntity, TProperty>> field, IEnumerable<TEnum> allowedValues)
            where TEnum : struct
        {
            List<string> allowedCodes = allowedValues.Select(x => x.ToString()).ToList();
            foreach (Control control in controls) { 
                control.SetFieldAllowedValues(field, allowedCodes);
            }

            return controls;
        }
    }
}
