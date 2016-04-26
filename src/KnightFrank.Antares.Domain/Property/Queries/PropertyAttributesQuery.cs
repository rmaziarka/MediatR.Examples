namespace KnightFrank.Antares.Domain.Property.Queries
{
    using System;
    
    using KnightFrank.Antares.Domain.Property.QueryResults;

    using MediatR;

    public class PropertyAttributesQuery : IRequest<PropertyAttributesQueryResult>
    {
        public Guid CountryId { get; set; }
        public Guid PropertyTypeId { get; set; }
    }
}
