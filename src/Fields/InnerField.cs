namespace Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using FluentValidation;


    public class InnerField
    {
        private readonly MemberInfo member;
        private readonly Func<object, object> compiled;
        private readonly LambdaExpression expression;
        private readonly Type containerType;
        private readonly Type propertyType;
        private readonly IList<IValidator> validators;
        public InnerField(MemberInfo member, Func<object, object> compiled, LambdaExpression expression, Type containerType, Type propertyType)
        {
            this.member = member;
            this.compiled = compiled;
            this.expression = expression;
            this.containerType = containerType;
            this.propertyType = propertyType;
            this.validators = new List<IValidator>();

            
        }

        public InnerField AddValidator(IValidator validator)
        {
            this.validators.Add(validator);
            return this;
        }

        public void Validate<T>(T obj)
        {
            var value = this.compiled(obj);
            
            foreach (IValidator validator in this.validators)
            {
                var result =validator.Validate(obj);
                if (!result.IsValid)
                {
                    throw new Exception();
                }

            }
        }
    }
}