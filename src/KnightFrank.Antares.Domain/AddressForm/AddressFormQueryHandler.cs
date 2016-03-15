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
            Country country = this.countryRepository.Get().SingleOrDefault(c => c.Code == message.CountryCode);

            if (country == null)
            {
                throw new DomainValidationException("message.CountryCode");
            }

            EnumTypeItem enumTypeItem = this.enumTypeItemRepository.Get().SingleOrDefault(i => i.Code == message.EntityType && i.EnumType.Code == "EntityType");

            if (enumTypeItem == null)
            {
                throw new DomainValidationException("message.EntityType");
            }

            AddressForm addressForm = null;

            addressForm = this.addressFormEntityTypeRepository.Get()
                    .Where(
                        addressFormEntityType =>
                        addressFormEntityType.EnumTypeItem.EnumType.Code == message.EntityType
                        && addressFormEntityType.AddressForm.Country.Code == message.CountryCode)
                    .Select(addressFormEntityType => addressFormEntityType.AddressForm)
                    .SingleOrDefault();

            if (addressForm == null)
            {
                IQueryable<AddressForm> query =
                    from af in addressFormRepository.Get().Where(i => i.CountryId == country.Id)
                    join afet in addressFormEntityTypeRepository.Get().Where(et => et.EnumTypeItemId == enumTypeItem.Id) on af
                        equals afet.AddressForm into result
                    from afet in result.DefaultIfEmpty()
                    where afet == null
                    select af;

                addressForm = query.Include(q => q.AddressFieldDefinitions).SingleOrDefault();
            }

            return AutoMapper.Mapper.Map<AddressFormQueryResult>(addressForm);
        }
    }
}
