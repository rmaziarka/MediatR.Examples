namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using KnightFrank.Antares.Domain.Common.Commands;

    public interface IAddressValidator
    {
        void Validate(CreateOrUpdateAddress address);
    }
}