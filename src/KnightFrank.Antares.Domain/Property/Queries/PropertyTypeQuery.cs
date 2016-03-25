namespace KnightFrank.Antares.Domain.Property.Queries
{
    using KnightFrank.Antares.Domain.Property.QueryResults;

    using MediatR;
    public class PropertyTypeQuery : IRequest<PropertyTypeQueryResult>
    {
        public string CountryCode { get; set; }
        public string DivisionCode { get; set; }
        public string LocaleCode { get; set; }
    }
}
