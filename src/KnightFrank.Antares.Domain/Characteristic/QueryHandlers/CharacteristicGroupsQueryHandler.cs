namespace KnightFrank.Antares.Domain.Characteristic.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Characteristic.Queries;

    using MediatR;

    public class CharacteristicGroupsQueryHandler :
        IRequestHandler<CharacteristicGroupsQuery, IEnumerable<CharacteristicGroupUsage>>
    {
        private readonly IReadGenericRepository<CharacteristicGroupUsage> characteristicGroupUsageRepository;

        public CharacteristicGroupsQueryHandler(
            IReadGenericRepository<CharacteristicGroupUsage> characteristicGroupUsageRepository)
        {
            this.characteristicGroupUsageRepository = characteristicGroupUsageRepository;
        }

        public IEnumerable<CharacteristicGroupUsage> Handle(CharacteristicGroupsQuery query)
        {
            List<CharacteristicGroupUsage> characteristicGroupUsages =
                this.characteristicGroupUsageRepository.GetWithInclude(
                    u => u.CharacteristicGroupItems.Select(x => x.Characteristic))
                    .Where(u => u.PropertyTypeId == query.PropertyTypeId)
                    .Where(u => u.Country.IsoCode == query.CountryCode)
                    .ToList();

            return characteristicGroupUsages;
        }
    }
}
