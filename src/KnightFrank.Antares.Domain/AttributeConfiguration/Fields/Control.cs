namespace KnightFrank.Antares.Domain.AttributeConfiguration.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    public class Control
    {
        public readonly ControlCode ControlCode;
        public readonly IList<InnerField> Fields;
        public readonly PageType PageType;

        public bool IsReadonly(object entity) => entity != null && ((bool?)this.isReadonlyExpression?.DynamicInvoke(entity) ?? false);
        public bool IsHidden(object entity) => entity != null && ((bool?)this.isHiddenExpression?.DynamicInvoke(entity) ?? false);

        private Delegate isHiddenExpression;
        private Delegate isReadonlyExpression;

        public Control(PageType pageType, ControlCode controlCode, IList<InnerField> fields)
        {
            this.ControlCode = controlCode;
            this.Fields = fields;
            this.PageType = pageType;
        }

        public Control(PageType pageType, ControlCode controlCode, InnerField create)
        {
            this.Fields = new List<InnerField> { create };
            this.ControlCode = controlCode;
            this.PageType = pageType;
        }

        public void SetReadonlyRule(LambdaExpression expression)
        {
            this.isReadonlyExpression = expression.Compile();
        }

        public void SetHiddenRule(LambdaExpression expression)
        {
            this.isHiddenExpression = expression.Compile();
        }

        public void SetFieldHiddenRule(LambdaExpression fieldExpression, LambdaExpression expression)
        {
            this.SetFieldExpression(fieldExpression, expression, false);
        }

        public void SetFieldReadonlyRule(LambdaExpression fieldExpression, LambdaExpression expression)
        {
            this.SetFieldExpression(fieldExpression, expression, true);
        }

        public void SetFieldAllowedValues(LambdaExpression fieldExpression, IList<string> allowedCodes)
        {
            foreach (InnerDictionaryField innerField in this.Fields.OfType<InnerDictionaryField>())
            {
                if (innerField.Expression.ToString() == fieldExpression.ToString())
                {
                    innerField.AllowedCodes = allowedCodes;
                }
            }
        }

        private void SetFieldExpression(LambdaExpression fieldExpression, LambdaExpression expression, bool readonlyExpression)
        {
            foreach (InnerField innerField in this.Fields)
            {
                if (innerField.Expression.ToString() == fieldExpression.ToString())
                {
                    if (readonlyExpression)
                    {
                        innerField.SetReadonlyRule(expression);
                    }
                    else
                    {
                        innerField.SetHiddenRule(expression);
                    }
                }
            }
        }

        public IList<InnerFieldState> GetFieldStates(object entity)
        {
            IList<InnerFieldState> fieldStates = new List<InnerFieldState>();
            foreach (InnerField field in this.Fields)
            {
                var state = new InnerFieldState
                {
                    PropertyType = field.PropertyType,
                    ContainerType = field.ContainerType,
                    Compiled = field.Compiled,
                    Validators = field.Validators,
                    Expression = field.Expression,
                    Required = field.Required,
                    Readonly = this.IsReadonly(entity) || field.IsReadonly(entity),
                    Hidden = this.IsHidden(entity) || field.IsHidden(entity),
                    Name = field.Expression.GetMemberName(),
                    ControlCode = this.ControlCode,
                    AllowedCodes = (field as InnerDictionaryField)?.AllowedCodes
                };

                fieldStates.Add(state);
            }

            return fieldStates;
        }

        public Control Copy()
        {
            List<InnerField> fieldCopies = this.Fields.Select(x => x.Copy()).ToList();
            var newControl = new Control(this.PageType, this.ControlCode, fieldCopies)
            {
                isHiddenExpression = this.isHiddenExpression,
                isReadonlyExpression = this.isReadonlyExpression
            };

            return newControl;
        }
    }
}