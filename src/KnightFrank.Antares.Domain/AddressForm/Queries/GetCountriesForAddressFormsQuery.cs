namespace KnightFrank.Antares.Domain.AddressForm.Queries
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.AddressForm.QueryResults;

    using MediatR;

    public class GetCountriesForAddressFormsQuery : IRequest<List<CountryLocalisedResult>>
    {
        public string LocaleIsoCode { get; set; }

        public string EntityType { get; set; }
    }
}