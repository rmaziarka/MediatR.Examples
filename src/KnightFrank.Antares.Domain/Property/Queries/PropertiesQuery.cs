namespace KnightFrank.Antares.Domain.Property.Queries
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Property;

    using MediatR;

    public class PropertiesQuery : IRequest<IEnumerable<Property>>
    {
    }
}
