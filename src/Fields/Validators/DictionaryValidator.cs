namespace Fields.Validators
{
    using System;

    public class DictionaryValidator<TEntity> : IValidator
    {
        public DictionaryValidator(string dictionaryCode)
        {
            Console.WriteLine("define DictionaryValidator: " + dictionaryCode);
        }

        public bool IsValid(TEntity entity, int value)
        {
            Console.WriteLine("DictionaryCode: " + value);
            return true;
        }

        public bool IsValid(object entity, object value)
        {
            Console.WriteLine("DictionaryCode: " + value);
            return true;
        }
    }
}