namespace KnightFrank.Antares.Domain.Enum.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Enum.Queries;
    using KnightFrank.Antares.Domain.Enum.QueryResults;

    using MediatR;

    public class EnumQueryHandler : IRequestHandler<EnumQuery, EnumQueryResult>
    {
        private readonly IReadGenericRepository<EnumLocalised> enumLocalisedRepository;

        public EnumQueryHandler(IReadGenericRepository<EnumLocalised> enumLocalisedRepository)
        {
            this.enumLocalisedRepository = enumLocalisedRepository;
        }

        public EnumQueryResult Handle(EnumQuery message)
        {
            IEnumerable<EnumLocalised> enumItems =
                this.enumLocalisedRepository.Get()
                    .Where(x => x.EnumTypeItem.EnumType.Code == message.Code && x.Locale.IsoCode == "en");

            return Mapper.Map<EnumQueryResult>(enumItems);
        }
    }
}
