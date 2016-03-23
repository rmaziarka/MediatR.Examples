namespace KnightFrank.Antares.Domain.AddressForm.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using AutoMapper;

    using global::Domain.Seedwork.Specification;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AddressForm.Queries;
    using KnightFrank.Antares.Domain.AddressForm.QueryResults;
    using KnightFrank.Antares.Domain.AddressForm.Specifications;
    using KnightFrank.Antares.Domain.Common.Exceptions;

    using MediatR;

    public class AddressFormQueryHandler : IRequestHandler<AddressFormQuery, AddressFormQueryResult>
    {
        private readonly IReadGenericRepository<AddressForm> addressFormRepository;

        private readonly IReadGenericRepository<Country> countryRepository;

        private readonly IReadGenericRepository<EnumTypeItem> enumTypeItemRepository;

        public AddressFormQueryHandler(
            IReadGenericRepository<AddressForm> addressFormRepository,
            IReadGenericRepository<Country> countryRepository,
            IReadGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.addressFormRepository = addressFormRepository;
            this.countryRepository = countryRepository;
            this.enumTypeItemRepository = enumTypeItemRepository;
        }

        public AddressFormQueryResult Handle(AddressFormQuery message)
        {
            // TODO custom exception
            Country country = this.countryRepository.Get().SingleOrDefault(c => c.IsoCode == message.CountryCode);

            if (country == null)
            {
                throw new DomainValidationException("message.CountryCode");
            }

            EnumTypeItem enumTypeItem =
                this.enumTypeItemRepository.Get()
                    .SingleOrDefault(i => i.Code == message.EntityType && i.EnumType.Code == "EntityType");

            if (enumTypeItem == null)
            {
                throw new DomainValidationException("message.EntityType");
            }

            Specification<AddressForm> isDefinedForEntityTypeSpecification =
                new IsAddressFormAssignedToCountry(country.Id) & new IsAddressFormAssignedToEntityType(enumTypeItem.Id);

            AddressForm addressForm =
                this.addressFormRepository.Get()
                    .Include(af => af.AddressFieldDefinitions.Select(afd => afd.AddressField))
                    .Include(af => af.AddressFieldDefinitions.Select(afd => afd.AddressFieldLabel))
                    .SingleOrDefault(isDefinedForEntityTypeSpecification.SatisfiedBy());

            if (addressForm == null)
            {
                Specification<AddressForm> isGloballyDefinedSpecification =
                    new IsAddressFormAssignedToCountry(country.Id) & new IsAddressFormGloballyDefined();

                addressForm =
                    this.addressFormRepository.Get()
                        .Include(af => af.AddressFieldDefinitions.Select(afd => afd.AddressField))
                        .Include(af => af.AddressFieldDefinitions.Select(afd => afd.AddressFieldLabel))
                        .SingleOrDefault(isGloballyDefinedSpecification.SatisfiedBy());
            }

            return Mapper.Map<AddressFormQueryResult>(addressForm);
        }
    }
}
