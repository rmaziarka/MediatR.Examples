namespace KnightFrank.Antares.Domain.Country.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Country.Queries;

    using MediatR;

    public class CountriesQueryHandler : IRequestHandler<CountriesQuery, IList<Country>>
    {
        private readonly IReadGenericRepository<Country> countryRepository;

        public CountriesQueryHandler(IReadGenericRepository<Country> countryRepository)
        {
            this.countryRepository = countryRepository;
        }
        
        public IList<Country> Handle(CountriesQuery message)
        {
            return this.countryRepository.Get().ToList();
        }
    }
}