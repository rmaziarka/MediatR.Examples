namespace KnightFrank.Antares.Domain.Activity.Queries
{
    using System;
    using System.Collections.Generic;
    
    using KnightFrank.Antares.Domain.Activity.QueryResults;

    using MediatR;

    public class RequirementTypeQuery : IRequest<IEnumerable<RequirementTypeQueryResult>>
    {
        public string CountryCode { get; set; }
    }
}
