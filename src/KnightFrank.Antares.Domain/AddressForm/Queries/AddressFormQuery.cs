namespace KnightFrank.Antares.Domain.AddressForm.Queries
{
    using KnightFrank.Antares.Domain.AddressForm.QueryResults;

    using MediatR;

    public class AddressFormQuery : IRequest<AddressFormQueryResult>
    {
        public string EntityType { get; set; }

        public string CountryCode { get; set; }
    }
}
