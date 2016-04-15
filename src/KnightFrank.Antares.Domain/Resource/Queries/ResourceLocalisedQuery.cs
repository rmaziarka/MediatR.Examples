namespace KnightFrank.Antares.Domain.Resource.Queries
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    public class ResourceLocalisedQuery : IRequest<Dictionary<Guid, string>>
    {
        public string IsoCode { get; set; }
    }
}
