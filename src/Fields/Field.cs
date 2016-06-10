namespace Fields
{
    using System;
    using System.Linq.Expressions;

    using Fields.Validators;

    using FluentValidation;
    using FluentValidation.Internal;

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

        public static Field<TEntity, int?> CreateDictionary(Expression<Func<TEntity, int?>> expression, string dictionaryCode)
        {
            return new Field<TEntity, int?>(expression, CreateInnerField(expression));
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