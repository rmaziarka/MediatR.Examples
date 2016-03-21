namespace KnightFrank.Antares.Domain.AddressForm.QueryHandlers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using AutoMapper;

    using global::Domain.Seedwork.Specification;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AddressForm.Queries;
    using KnightFrank.Antares.Domain.AddressForm.QueryResults;
    using KnightFrank.Antares.Domain.AddressForm.Specifications;
    using KnightFrank.Antares.Domain.Common.Specifications;

    using MediatR;

    public class GetCountriesForAddressFormQueryHandler : IRequestHandler<GetCountriesForAddressFormsQuery, List<CountryLocalisedResult>>
    {
        private readonly IReadGenericRepository<EnumTypeItem> enumTypeItemRepository;
        private readonly IReadGenericRepository<AddressForm> addressFormRepository;

        public GetCountriesForAddressFormQueryHandler(
            IReadGenericRepository<EnumTypeItem> enumTypeItemRepository,
            IReadGenericRepository<AddressForm> addressFormRepository)
        {
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.addressFormRepository = addressFormRepository;
        }

        public List<CountryLocalisedResult> Handle(GetCountriesForAddressFormsQuery message)
        {
            EnumTypeItem enumTypeItem = this.enumTypeItemRepository.Get().SingleOrDefault(i => i.Code == message.EntityType && i.EnumType.Code == "EntityType");

            if (enumTypeItem == null)
            {
                throw new DomainValidationException("message.EntityType");
            }

            // TODO lang code should be set in header
            if (string.IsNullOrWhiteSpace(message.LocaleIsoCode))
            {
                message.LocaleIsoCode = "en";
            }

            Specification<AddressForm> isAddressFormDefinedSpecification = 
                new IsAddressFormAssignedToEntityType(enumTypeItem.Id) | new IsAddressFormGloballyDefined();

            Specification<CountryLocalised> isLocaled = new IsLocaled<CountryLocalised>(message.LocaleIsoCode);

            List<CountryLocalised> countryLocaliseds = this.addressFormRepository.Get()
                                         .Where(isAddressFormDefinedSpecification.SatisfiedBy())
                                         .Select(i => i.Country)
                                         .Distinct()
                                         .SelectMany(i => i.CountryLocaliseds)
                                         .Where(isLocaled.SatisfiedBy())
                                         .Include(i => i.Locale)
                                         .Include(i => i.Country)
                                         .ToList();

            return Mapper.Map<List<CountryLocalised>, List<CountryLocalisedResult>>(countryLocaliseds);
        }
    }
}