namespace Fields.Validators
{
    using System;
    using System.Linq.Expressions;

    public class GreatherThan<TEntity, TValue> : IValidator
    {
        private readonly TValue limit;
        private readonly string messageDictionaryCode;
        private readonly Func<TEntity, TValue> getLimit;

        public GreatherThan(TValue limit, string dictionaryCode)
        {
            this.limit = limit;
            this.messageDictionaryCode = dictionaryCode;
            Console.WriteLine("define DictionaryValidator: " + dictionaryCode);
        }

        public GreatherThan(Expression<Func<TEntity, TValue>> propertySelector, TValue limit, string dictionaryCode)
        {
            this.limit = limit;
            this.messageDictionaryCode = dictionaryCode;
            this.getLimit = propertySelector.Compile();
        }

        public bool IsValid(TEntity entity, TValue value)
        {
            TValue limitValue = this.getLimit != null ? this.getLimit(entity) : this.limit;
            Console.WriteLine("GraterThan Is valid (limit: {0}, value: {1}), message: {2}", limitValue, value, this.messageDictionaryCode);
            return true;
        }

        public bool IsValid(object entity, object value)
        {
            throw new NotImplementedException();
        }
    }
}