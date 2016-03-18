namespace KnightFrank.Antares.Domain.AddressForm.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AddressForm.Queries;
    using KnightFrank.Antares.Domain.AddressForm.QueryResults;

    using MediatR;

    public class AddressFormQueryHandler : IRequestHandler<AddressFormQuery, AddressFormQueryResult>
    {
        private readonly IReadGenericRepository<AddressFormEntityType> addressFormEntityTypeRepository;

        private readonly IReadGenericRepository<AddressForm> addressFormRepository;

        private readonly IReadGenericRepository<Country> countryRepository;

        private readonly IReadGenericRepository<EnumTypeItem> enumTypeItemRepository;

        public AddressFormQueryHandler(
            IReadGenericRepository<AddressForm> addressFormRepository,
            IReadGenericRepository<AddressFormEntityType> addressFormEntityTypeRepository,
            IReadGenericRepository<Country> countryRepository,
            IReadGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.addressFormRepository = addressFormRepository;
            this.addressFormEntityTypeRepository = addressFormEntityTypeRepository;
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

            AddressForm addressForm =
                this.addressFormRepository.Get()
                    .Include(af => af.AddressFieldDefinitions)
                    .SingleOrDefault(
                        af =>
                        af.CountryId == country.Id && af.AddressFormEntityTypes.Any(afet => afet.EnumTypeItemId == enumTypeItem.Id));

            if (addressForm == null)
            {
                addressForm =
                    this.addressFormRepository.Get()
                        .Include(af => af.AddressFieldDefinitions)
                        .SingleOrDefault(af => af.CountryId == country.Id && !af.AddressFormEntityTypes.Any());
            }

            return Mapper.Map<AddressFormQueryResult>(addressForm);
        }
    }
}
