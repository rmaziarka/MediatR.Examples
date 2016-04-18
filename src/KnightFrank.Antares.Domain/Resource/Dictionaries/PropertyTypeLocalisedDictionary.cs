namespace KnightFrank.Antares.Domain.Resource.Dictionaries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;

    public class PropertyTypeLocalisedDictionary : IResourceLocalisedDictionary
    {
        private readonly IReadGenericRepository<PropertyTypeLocalised> propertyTypeLocalisedRepository;

        public PropertyTypeLocalisedDictionary(
            IReadGenericRepository<PropertyTypeLocalised> propertyTypeLocalisedRepository)
        {
            this.propertyTypeLocalisedRepository = propertyTypeLocalisedRepository;
        }

        public Dictionary<Guid, string> GetDictionary(string isoCode)
        {
            Dictionary<Guid, string> dictionary =
                this.propertyTypeLocalisedRepository.Get()
                    .Where(el => el.Locale.IsoCode == isoCode)
                    .ToDictionary(el => el.PropertyTypeId, el => el.Value);

            return dictionary;
        }
    }
}
