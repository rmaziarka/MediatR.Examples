namespace KnightFrank.Antares.Domain.Common
{
    using FluentValidation;

    public interface IDomainValidator<in T> : IValidator<T>
    {
    }
}