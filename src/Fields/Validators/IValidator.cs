namespace Fields.Validators
{
    public interface IValidator
    {
        bool IsValid(object entity, object value);
    }
}