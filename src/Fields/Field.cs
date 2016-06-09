namespace Fields
{
    using System;
    using System.Linq.Expressions;

    using Fields.Extensions;
    using Fields.Validators;

    public class Field<TEntity>
    {
        public static InnerField Create<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            return CreateInnerField(expression);
        }

        public static InnerField CreateText<TProperty>(Expression<Func<TEntity, TProperty>> expression, int length)
        {
            var innerField = CreateInnerField(expression);
            //innerField.AddValidator(new StringLengthValidator<TEntity>(length));
            return innerField;
        }

        public static InnerField CreateDictionary<TProperty>(Expression<Func<TEntity, TProperty>> expression, string dictionaryCode)
        {
            var innerField = CreateInnerField(expression);
            //innerField.AddValidator(new DictionaryValidator<TEntity>(dictionaryCode));
            return innerField;
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