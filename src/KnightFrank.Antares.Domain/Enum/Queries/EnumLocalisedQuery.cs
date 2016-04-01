namespace KnightFrank.Antares.Domain.Enum.Queries
{
    using System;
    using System.Collections.Generic;

    using MediatR;

    public class EnumLocalisedQuery : IRequest<Dictionary<Guid, string>>
    {
        public string IsoCode { get; set; }
    }
}
