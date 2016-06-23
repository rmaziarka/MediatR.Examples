namespace KnightFrank.Antares.Domain.AttributeConfiguration.Fields
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using FluentValidation;
    using FluentValidation.Internal;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions.Fields;

    public class Field<TEntity, TProperty> : IField
    {
        public Expression<Func<TEntity, TProperty>> Selector;
        public InnerField InnerField { get; }

        public IField Copy()
        {
            InnerField innerFieldCopy = this.InnerField.Copy();
            var copyField = new Field<TEntity, TProperty>(this.Selector, innerFieldCopy);

            return copyField;
        }

        public Field(Expression<Func<TEntity, TProperty>> selector, InnerField field)
        {
            this.Selector = selector;
            this.InnerField = field;
        }

        public void SetRequired()
        {
            this.InnerField.Required = true;
            this.InnerField.AddValidator(new EntityValidator<TEntity>(v => v.RuleFor(this.Selector).NotEmpty().NotNull()));
        }
    }

    public static class Field<TEntity>
    {
        public static Field<TEntity, TProperty> Create<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            return new Field<TEntity, TProperty>(expression, CreateInnerField(expression));
        }

        public static IList<IField> Create<TProperty, TProperty2>(Expression<Func<TEntity, TProperty>> expression, Expression<Func<TEntity, TProperty2>> associatedExpression = null)
        {
            return new List<IField>
            {
                new Field<TEntity, TProperty>(expression, CreateInnerField(expression)),
                new Field<TEntity, TProperty2>(associatedExpression, CreateInnerField(associatedExpression))
            };
        }

        public static Field<TEntity, string> CreateText(Expression<Func<TEntity, string>> expression, int maxLength)
        {
            return new Field<TEntity, string>(expression, CreateInnerField(expression)).Length(maxLength);
        }

        public static Field<TEntity, string> CreateText(Expression<Func<TEntity, string>> expression, int minLength, int maxLength)
        {
            return new Field<TEntity, string>(expression, CreateInnerField(expression)).Length(minLength, maxLength);
        }

        public static Field<TEntity, Guid> CreateDictionary(Expression<Func<TEntity, Guid>> expression, string dictionaryCode)
        {
            return new Field<TEntity, Guid>(expression, CreateInnerDictionaryField(expression, dictionaryCode));
        }

        public static Field<TEntity, Guid?> CreateDictionary(Expression<Func<TEntity, Guid?>> expression, string dictionaryCode)
        {
            return new Field<TEntity, Guid?>(expression, CreateInnerDictionaryField(expression, dictionaryCode));
        }

        private static InnerField CreateInnerField<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            MemberInfo member = expression.GetMember();
            Func<TEntity, TProperty> compiled = expression.Compile();
            var innerField = new InnerField(member, compiled.CoerceToNonGeneric(), expression, typeof(TEntity), typeof(TProperty));
            return innerField;
        }

        private static InnerField CreateInnerDictionaryField<TProperty>(Expression<Func<TEntity, TProperty>> expression, string dictionaryCode)
        {
            MemberInfo member = expression.GetMember();
            Func<TEntity, TProperty> compiled = expression.Compile();
            var innerField = new InnerDictionaryField(member, compiled.CoerceToNonGeneric(), expression, typeof(TEntity), typeof(TProperty), dictionaryCode);
            return innerField;
        }
    }
}