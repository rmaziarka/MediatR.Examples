namespace KnightFrank.Antares.Domain.Enum.QueryHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Enum.Queries;

    using MediatR;

    public class EnumLocalisedQueryHandler : IRequestHandler<EnumLocalisedQuery, Dictionary<Guid, string>>
    {
        private readonly IReadGenericRepository<EnumLocalised> enumLocalisedRepository;

        public EnumLocalisedQueryHandler(IReadGenericRepository<EnumLocalised> enumLocalisedRepository)
        {
            this.enumLocalisedRepository = enumLocalisedRepository;
        }

        public Dictionary<Guid, string> Handle(EnumLocalisedQuery query)
        {
            Dictionary<Guid, string> dictionary =
                this.enumLocalisedRepository.Get()
                    .Where(el => el.Locale.IsoCode == query.IsoCode)
                    .ToDictionary(el => el.EnumTypeItemId, el => el.Value);

            return dictionary;
        }
    }
}
