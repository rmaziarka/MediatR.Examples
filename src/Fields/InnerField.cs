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
        public readonly LambdaExpression expression;
        private readonly Type containerType;
        private readonly Type propertyType;
        private readonly IList<IValidator> validators;

        private Delegate isHiddenExpression;
        private Delegate isReadonlyExpression;

        public bool IsReadonly(object entity) => entity != null && ((bool?)this.isReadonlyExpression?.DynamicInvoke(entity) ?? false);
        public bool IsHidden(object entity) => entity != null && ((bool?)this.isHiddenExpression?.DynamicInvoke(entity) ?? false);


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
            foreach (IValidator validator in this.validators)
            {
                var result = validator.Validate(obj);
                if (!result.IsValid)
                {
                    Console.WriteLine("Validator: " + validator.ToString());
                }

            }
        }

        public void SetReadonlyRule(LambdaExpression readonlyExpression)
        {
            this.isReadonlyExpression = readonlyExpression.Compile();
        }

        public void SetHiddenRule(LambdaExpression hiddenExpression)
        {
            this.isHiddenExpression = hiddenExpression.Compile();
        }
    }
}