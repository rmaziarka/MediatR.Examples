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
        public readonly PageType PageType;
        private IList<InnerField> fields;

        public bool IsReadonly(object entity) => entity != null && ((bool?)this.isReadonlyExpression?.DynamicInvoke(entity) ?? false);
        public bool IsHidden(object entity) => entity != null && ((bool?)this.isHiddenExpression?.DynamicInvoke(entity) ?? false);

        private Delegate isHiddenExpression;
        private Delegate isReadonlyExpression;

        public Control(PageType pageType, ControlCode controlCode)
        {
            this.ControlCode = controlCode;
            this.PageType = pageType;
            this.fields = new List<InnerField>();
        }

        public void AddField(InnerField field)
        {
            this.fields.Add(field);
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
            foreach (InnerDictionaryField innerField in this.fields.OfType<InnerDictionaryField>())
            {
                if (innerField.Expression.ToString() == fieldExpression.ToString())
                {
                    innerField.AllowedCodes = allowedCodes;
                }
            }
        }

        private void SetFieldExpression(LambdaExpression fieldExpression, LambdaExpression expression, bool readonlyExpression)
        {
            foreach (InnerField innerField in this.fields)
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
            foreach (InnerField field in this.fields)
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
            var newControl = new Control(this.PageType, this.ControlCode)
            {
                isHiddenExpression = this.isHiddenExpression,
                isReadonlyExpression = this.isReadonlyExpression
            };

            if (this.fields.Any())
            {
                newControl.fields = this.fields.Select(x => x.Copy()).ToList();
            }

            return newControl;
        }
    }
}