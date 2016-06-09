namespace Fields.Extensions
{
    using Validators;

    public static class FieldExtensions
    {
        public static InnerField GreaterThan(this InnerField field, decimal value)
        {
            /*var validator = new GreatherThan(value, "greater than - error message");
            return field.AddValidator(validator);*/
            return field;
        }
    }
}
