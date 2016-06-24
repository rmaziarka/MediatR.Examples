namespace KnightFrank.Antares.Domain.Country.Queries
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Resource;

    using MediatR;

    public class CountriesQuery : IRequest<IList<Country>>
    {
    }
}