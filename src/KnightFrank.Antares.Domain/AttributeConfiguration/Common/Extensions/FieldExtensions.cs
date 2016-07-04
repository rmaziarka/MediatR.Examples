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
        public static Field<TEntity, TProperty> ExternalValidator<TEntity, TProperty>(this Field<TEntity, TProperty> field, AbstractValidator<TProperty> externalValidator)
        {
            field.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(field.Selector).SetValidator(externalValidator)));
            return field;
        }

        public static Field<TEntity, IList<TProperty>> ExternalCollectionValidator<TEntity, TProperty>(this Field<TEntity, IList<TProperty>> field, AbstractValidator<TProperty> externalValidator)
        {
            field.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(field.Selector).SetCollectionValidator(externalValidator)));
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

        public static IList<Control> FieldHasAllowed<TEntity, TProperty, TEnum>(this IList<Control> controls, Expression<Func<TEntity, TProperty>> field, IEnumerable<TEnum> allowedValues) where TEnum : struct
        {
            return SetAllowedValues(controls, field, allowedValues);
        }

        public static IList<Field<TEntity, TProperty>> Field<TEntity, TProperty>(this IList<Tuple<Control, IList<IField>>> controlFields, Expression<Func<TEntity, TProperty>> fieldSelector)
        {
            var fields = new List<Field<TEntity, TProperty>>();
            foreach (Tuple<Control, IList<IField>> controlField in controlFields)
            {
                foreach (IField field in controlField.Item2)
                {
                    if (field.InnerField.ContainerType == typeof(TEntity) && field.InnerField.Expression.ToString() == fieldSelector.ToString())
                    {
                        fields.Add(field as Field<TEntity, TProperty>);
                    }
                }
            }

            return fields;
        }

        public static IList<Tuple<Control, IList<IField>>> Fields<TEntity, TProperty>(this IList<Tuple<Control, IList<IField>>> controlFields, FieldActions<TEntity, TProperty> fieldActions)
        {
            foreach (KeyValuePair<Expression<Func<TEntity, TProperty>>, Action<IList<Field<TEntity, TProperty>>>> fieldAction in fieldActions)
            {
                fieldAction.Value(controlFields.Field(fieldAction.Key));
            }

            return controlFields;
        }

        public static IList<Tuple<Control, IList<IField>>> Hidden(this IList<Tuple<Control, IList<IField>>> controlFields)
        {
            foreach (Tuple<Control, IList<IField>> tuple in controlFields)
            {
                tuple.Item1.SetHidden();
            }

            return controlFields;
        }

        public static IList<Tuple<Control, IList<IField>>> Readonly(this IList<Tuple<Control, IList<IField>>> controlFields)
        {
            foreach (Tuple<Control, IList<IField>> tuple in controlFields)
            {
                tuple.Item1.SetReadonly();
            }

            return controlFields;
        }

        public static IList<Tuple<Control, IList<IField>>> ReadonlyWhen<TEntity>(this IList<Tuple<Control, IList<IField>>> controlFields, Expression<Func<TEntity, bool>> expression )
        {
            IEnumerable<Tuple<Control, IList<IField>>> controlsForCurrentType = 
                controlFields.Where(x => x.Item2.Any(f => f.InnerField.ContainerType == typeof(TEntity)));

            foreach (Tuple<Control, IList<IField>> tuple in controlsForCurrentType)
            {
                tuple.Item1.SetReadonlyRule(expression);
            }

            return controlFields;
        }

        public static IList<Tuple<Control, IList<IField>>> HiddenWhen<TEntity>(this IList<Tuple<Control, IList<IField>>> controlFields, Expression<Func<TEntity, bool>> expression)
        {
            IEnumerable<Tuple<Control, IList<IField>>> controlsForCurrentType =
                controlFields.Where(x => x.Item2.Any(f => f.InnerField.ContainerType == typeof(TEntity)));

            foreach (Tuple<Control, IList<IField>> tuple in controlsForCurrentType)
            {
                tuple.Item1.SetHiddenRule(expression);
            }

            return controlFields;
        }

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
            foreach (Control control in controls)
            {
                control.SetFieldAllowedValues(field, allowedCodes);
            }

            return controls;
        }
    }
}
