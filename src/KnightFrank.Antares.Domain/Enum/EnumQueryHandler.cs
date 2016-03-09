namespace KnightFrank.Antares.Domain.Enum
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;

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
            IEnumerable<EnumLocalised> enumItems = this.enumLocalisedRepository.FindBy(x => x.EnumTypeItem.EnumType.Code == message.Code && x.Locale.IsoCode == "en");

            return new EnumQueryResult { Items = enumItems.Select(x => new EnumQueryItemResult { Id = x.Id, Value = x.Value }) };
        }
    }
}