namespace KnightFrank.Antares.Domain.AddressForm
{
    using MediatR;

    public class AddressFormQuery : IRequest<AddressFormQueryResult>
    {
        public string EntityType { get; set; }

        public string CountryCode { get; set; }
    }
}
