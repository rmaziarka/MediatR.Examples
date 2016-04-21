namespace KnightFrank.Antares.Domain.Activity.Queries
{
    using System;
    using System.Collections.Generic;
    
    using KnightFrank.Antares.Domain.Activity.QueryResults;

    using MediatR;

    public class ActivityTypeQuery : IRequest<IEnumerable<ActivityTypeQueryResult>>
    {
        public string CountryCode { get; set; }
        public Guid PropertyTypeId { get; set; }
    }
}
