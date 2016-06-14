namespace KnightFrank.Antares.Domain.AttributeConfiguration.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using FluentValidation;

    public class InnerField
    {
        protected readonly MemberInfo Member;
        public readonly Func<object, object> Compiled;
        public readonly LambdaExpression Expression;
        public readonly Type ContainerType;
        public readonly Type PropertyType;
        public readonly IList<IValidator> Validators;

        protected Delegate IsHiddenExpression;
        protected Delegate IsReadonlyExpression;

        public bool IsReadonly(object entity) => entity != null && ((bool?)this.IsReadonlyExpression?.DynamicInvoke(entity) ?? false);
        public bool IsHidden(object entity) => entity != null && ((bool?)this.IsHiddenExpression?.DynamicInvoke(entity) ?? false);

        public bool Required { get; set; }

        public InnerField(MemberInfo member, Func<object, object> compiled, LambdaExpression expression, Type containerType, Type propertyType)
        {
            this.Member = member;
            this.Compiled = compiled;
            this.Expression = expression;
            this.ContainerType = containerType;
            this.PropertyType = propertyType;
            this.Validators = new List<IValidator>();
        }

        public InnerField AddValidator(IValidator validator)
        {
            this.Validators.Add(validator);
            return this;
        }

        public void Validate<T>(T obj)
        {
            foreach (IValidator validator in this.Validators)
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
            this.IsReadonlyExpression = readonlyExpression.Compile();
        }

        public void SetHiddenRule(LambdaExpression hiddenExpression)
        {
            this.IsHiddenExpression = hiddenExpression.Compile();
        }

        public virtual InnerField Copy()
        {
            var fieldCopy = new InnerField(this.Member, this.Compiled, this.Expression, this.ContainerType, this.PropertyType)
            {
                IsHiddenExpression = this.IsHiddenExpression,
                IsReadonlyExpression = this.IsReadonlyExpression,
                Required = this.Required
            };

            foreach (IValidator validator in this.Validators)
            {
                fieldCopy.AddValidator(validator);
            }

            return fieldCopy;
        }
    }
}