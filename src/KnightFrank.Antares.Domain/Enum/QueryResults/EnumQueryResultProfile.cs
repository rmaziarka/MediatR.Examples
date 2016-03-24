namespace KnightFrank.Antares.Domain.Enum.QueryResults
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Enum;

    public class EnumQueryResultProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<EnumLocalised, EnumQueryItemResult>()
                .ForMember(dest => dest.Id, options => options.MapFrom(sourceMember => sourceMember.EnumTypeItemId));

            this.CreateMap<IEnumerable<EnumLocalised>, EnumQueryResult>()
                .ForMember(dest => dest.Items, options => options.MapFrom(sourceMember => sourceMember.ToList()));
        }
    }
}
