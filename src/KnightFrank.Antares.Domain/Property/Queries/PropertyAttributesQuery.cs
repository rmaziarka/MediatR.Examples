namespace KnightFrank.Antares.Domain.Property.Queries
{
    using System;
    
    using KnightFrank.Antares.Domain.Property.QueryResults;

    using MediatR;

    public class PropertyAttributesQuery : IRequest<PropertyAttributesQueryResult>
    {
        public string CountryCode { get; set; }
        public Guid PropertyTypeId { get; set; }
    }
}
