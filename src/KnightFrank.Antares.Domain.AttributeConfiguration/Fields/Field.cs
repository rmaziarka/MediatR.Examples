namespace KnightFrank.Antares.Domain.AttributeConfiguration.Fields
{
    using System;
    using System.Linq.Expressions;

    using FluentValidation;
    using FluentValidation.Internal;

    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;

    public class Field<TEntity, TProperty>
    {
        public Expression<Func<TEntity, TProperty>> Selector;
        public InnerField InnerField;

        public Field(Expression<Func<TEntity, TProperty>> selector, InnerField field)
        {
            this.Selector = selector;
            this.InnerField = field;
        }
    }

    public class Field<TEntity>
    {
        public static Field<TEntity, TProperty> Create<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            return new Field<TEntity, TProperty>(expression, CreateInnerField(expression));

        }

        public static Field<TEntity, string> CreateText(Expression<Func<TEntity, string>> expression, int length)
        {
            var innerField = CreateInnerField(expression);
            innerField.AddValidator(new EntityValidator<TEntity>(x => x.RuleFor(expression).Length(length)));
            return new Field<TEntity, string>(expression, innerField);
        }

        public static Field<TEntity, Guid?> CreateDictionary(Expression<Func<TEntity, Guid?>> expression, string dictionaryCode)
        {
            //TODO: support dictionary + validation
            return new Field<TEntity, Guid?>(expression, CreateInnerField(expression));
        }

        private static InnerField CreateInnerField<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            var member = expression.GetMember();
            var compiled = expression.Compile();
            var innerField = new InnerField(member, compiled.CoerceToNonGeneric(), expression, typeof(TEntity),
                typeof(TProperty));
            return innerField;
        }
    }
}