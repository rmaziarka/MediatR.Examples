namespace Fields.Validators
{
    using System;

    public class StringLengthValidator<TEntity> : IValidator
    {
        private readonly int maxLength;

        public StringLengthValidator(int length)
        {
            this.maxLength = length;
            Console.WriteLine("StringLengthValidator: " + length);
        }

        public bool IsValid(TEntity entity, string value)
        {
            Console.WriteLine("StringLengthValidator: " + value);
            return true;
        }

        public bool IsValid(object entity, object value)
        {
            Console.WriteLine("StringLengthValidator: " + value);
            return true;
        }
    }
}