namespace KnightFrank.Antares.Domain.AttributeConfiguration.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using FluentValidation;

    public class InnerDictionaryField : InnerField
    {
        public readonly string DictionaryCode;
        public IList<string> AllowedCodes ;

        public InnerDictionaryField(MemberInfo member, Func<object, object> compiled, LambdaExpression expression, Type containerType, Type propertyType, string dictionaryCode) 
            : base(member, compiled, expression, containerType, propertyType)
        {
            this.DictionaryCode = dictionaryCode;
        }

        public override InnerField Copy()
        {
            var fieldCopy = new InnerDictionaryField(this.Member, this.Compiled, this.Expression, this.ContainerType, this.PropertyType, this.DictionaryCode)
            {
                IsHiddenExpression = this.IsHiddenExpression,
                IsReadonlyExpression = this.IsReadonlyExpression,
                Required = this.Required
            };

            if (this.AllowedCodes != null)
            {
                fieldCopy.AllowedCodes=new List<string>(this.AllowedCodes);
            }

            foreach (IValidator validator in this.Validators)
            {
                fieldCopy.AddValidator(validator);
            }

            return fieldCopy;
        }
    }
}