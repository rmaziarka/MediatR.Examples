namespace KnightFrank.Antares.Domain.Resource.Dictionaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Repository;

    public class CountryLocalisedDictionary : IResourceLocalisedDictionary
    {
        private readonly IReadGenericRepository<CountryLocalised> countryLocalisedRepository;

        public CountryLocalisedDictionary(IReadGenericRepository<CountryLocalised> countryLocalisedRepository)
        {
            this.countryLocalisedRepository = countryLocalisedRepository;
        }

        public Dictionary<Guid, string> GetDictionary(string isoCode)
        {
            Dictionary<Guid, string> dictionary =
                this.countryLocalisedRepository.Get()
                    .Where(el => el.Locale.IsoCode == isoCode)
                    .ToDictionary(el => el.CountryId, el => el.Value);

            return dictionary;
        }
    }
}
