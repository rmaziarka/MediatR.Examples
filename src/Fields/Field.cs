namespace Fields
{
    using System;
    using System.Linq.Expressions;

    using Fields.Validators;

    using FluentValidation;
    using FluentValidation.Internal;

    public class Field<TEntity>
    {
        public static InnerField Create<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            return CreateInnerField(expression);
        }

        public static InnerField CreateText(Expression<Func<TEntity, string>> expression, int length)
        {
            var innerField = CreateInnerField(expression);
            return innerField.AddValidator(new EntityValidator<TEntity>(x => x.RuleFor(expression).Length(length)));
        }

        public static InnerField CreateDictionary(Expression<Func<TEntity, int?>> expression, string dictionaryCode)
        {
            return CreateInnerField(expression);
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