namespace KnightFrank.Antares.Domain.Requirement.Commands
{
    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;

    public class CreateRequirementCommandValidator : AbstractValidator<CreateRequirementCommand>
    {
        private readonly IGenericRepository<AddressForm> addressFormRepository;
        private readonly IGenericRepository<Country> countryRepository;

        public CreateRequirementCommandValidator(IGenericRepository<AddressForm> addressFormRepository, IGenericRepository<Country> countryRepository)
        {
            this.addressFormRepository = addressFormRepository;
            this.countryRepository = countryRepository;

            this.RuleFor(x => x.MaxPrice).GreaterThanOrEqualTo(x => x.MinPrice.GetValueOrDefault(0)).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MaxBedrooms).GreaterThanOrEqualTo(x => x.MinBedrooms.GetValueOrDefault(0)).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MaxReceptionRooms).GreaterThanOrEqualTo(x => x.MinReceptionRooms.GetValueOrDefault(0)).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MaxBathrooms).GreaterThanOrEqualTo(x => x.MinBathrooms.GetValueOrDefault(0)).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MaxParkingSpaces).GreaterThanOrEqualTo(x => x.MinParkingSpaces.GetValueOrDefault(0)).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MaxArea).GreaterThanOrEqualTo(x => x.MinArea.GetValueOrDefault(0)).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MaxLandArea).GreaterThanOrEqualTo(x => x.MinLandArea.GetValueOrDefault(0)).GreaterThanOrEqualTo(0);

            this.RuleFor(x => x.MinPrice).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MinBedrooms).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MinReceptionRooms).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MinBathrooms).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MinParkingSpaces).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MinArea).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.MinLandArea).GreaterThanOrEqualTo(0);

            this.RuleFor(x => x.Description).Length(0, 4000);
            this.RuleFor(x => x.Contacts).NotEmpty();

            this.Custom(this.AddressValid);
        }
        private ValidationFailure AddressValid(CreateRequirementCommand command)
        {
            Country country = this.countryRepository.GetById(command.Address.CountryId);

            if (country == null)
            {
                return new ValidationFailure(nameof(command.Address.CountryId), "Invalid country has been provided.");
            }

            AddressForm addressForm = this.addressFormRepository.GetById(command.Address.AddressFormId);

            if (addressForm == null)
            {
                return new ValidationFailure(nameof(command.Address.AddressFormId), "Invalid address form has been provided.");
            }

            return null;
        }
    }
}