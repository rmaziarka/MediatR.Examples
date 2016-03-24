namespace KnightFrank.Antares.Domain.Property.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Property;

    using MediatR;

    public class PropertyQuery : IRequest<Property>
    {
        public Guid Id { get; set; }
    }
}
