namespace KnightFrank.Antares.Domain.AddressForm
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;

    using MediatR;

    using Ninject.Extensions.Conventions.Syntax;

    public class AddressFormQueryHandler : IRequestHandler<AddressFormQuery, AddressFormQueryResult>
    {
        private readonly IReadGenericRepository<AddressForm> addressFormRepository;
        private readonly IReadGenericRepository<AddressFormEntityType> addressFormEntityTypeRepository;
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

            EnumTypeItem enumTypeItem = this.enumTypeItemRepository.Get().SingleOrDefault(i => i.Code == message.EntityType && i.EnumType.Code == "EntityType");

            if (enumTypeItem == null)
            {
                throw new DomainValidationException("message.EntityType");
            }

            AddressForm addressForm = this.addressFormRepository.Get()
                    .Include(af => af.AddressFieldDefinitions)
                    .SingleOrDefault(af =>
                        af.CountryId == country.Id &&
                        af.AddressFormEntityTypes.Any(afet => afet.EnumTypeItemId == enumTypeItem.Id));

            if (addressForm == null)
            {
                addressForm = this.addressFormRepository.Get()
                    .Include(af => af.AddressFieldDefinitions)
                    .SingleOrDefault(af =>
                        af.CountryId == country.Id &&
                        !af.AddressFormEntityTypes.Any());
            }

            return AutoMapper.Mapper.Map<AddressFormQueryResult>(addressForm);
        }
    }
}
