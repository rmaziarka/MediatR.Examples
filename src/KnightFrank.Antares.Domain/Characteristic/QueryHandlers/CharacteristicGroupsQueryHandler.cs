namespace KnightFrank.Antares.Domain.Characteristic.QueryHandlers
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.Characteristic.Queries;
    using MediatR;
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Repository;

    public class CharacteristicGroupsQueryHandler : IRequestHandler<CharacteristicGroupsQuery, IEnumerable<CharacteristicGroupUsage>>
    {
        private readonly IReadGenericRepository<CharacteristicGroupUsage> characteristicGroupUsageRepository;

        public CharacteristicGroupsQueryHandler(IReadGenericRepository<CharacteristicGroupUsage> characteristicGroupUsageRepository)
        {
            this.characteristicGroupUsageRepository = characteristicGroupUsageRepository;
        }

        public IEnumerable<CharacteristicGroupUsage> Handle(CharacteristicGroupsQuery message)
        {
            List<CharacteristicGroupUsage> characteristicGroupUsages = this.characteristicGroupUsageRepository
                                                 .GetWithInclude(u => u.CharacteristicGroupItems.Select(x => x.Characteristic))
                                                 .Where(u => u.PropertyTypeId == message.PropertyTypeId)
                                                 .Where(u => u.Country.IsoCode == message.CountryCode)
                                                 .ToList();

            return characteristicGroupUsages;
        }
    }
}