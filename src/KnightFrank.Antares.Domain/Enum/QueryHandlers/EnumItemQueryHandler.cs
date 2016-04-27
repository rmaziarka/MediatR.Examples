namespace KnightFrank.Antares.Domain.Enum.QueryHandlers
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Enum.Queries;
    using KnightFrank.Antares.Domain.Enum.QueryResults;

    using MediatR;

    public class EnumItemQueryHandler : IRequestHandler<EnumItemQuery, Dictionary<string, ICollection<EnumItemResult>>>
    {
        private readonly IReadGenericRepository<EnumType> enumTypeRepository;

        public EnumItemQueryHandler(IReadGenericRepository<EnumType> enumTypeRepository)
        {
            this.enumTypeRepository = enumTypeRepository;
        }

        public Dictionary<string, ICollection<EnumItemResult>> Handle(EnumItemQuery message)
        {
            Dictionary<string, ICollection<EnumItemResult>> dictionary =
                this.enumTypeRepository.GetWithInclude(et => et.EnumTypeItems)
                    .ToDictionary(et => et.Code, et => this.Map(et.EnumTypeItems));

            return dictionary;
        }

        private ICollection<EnumItemResult> Map(ICollection<EnumTypeItem> enumTypeItems)
        {
            return enumTypeItems.Select(eti => Mapper.Map<EnumItemResult>(eti)).ToList();
        }
    }
}
