namespace KnightFrank.Antares.Domain.Activity.Queries
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Portal;

    using MediatR;

    public class ActivityPortalsQuery : IRequest<IEnumerable<Portal>>
    {
        public string CountryCode { get; set; }
    }
}
