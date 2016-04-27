namespace KnightFrank.Antares.Domain.Enum.QueryResults
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Enum;

    public class EnumItemResultProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<EnumTypeItem, EnumItemResult>();
        }
    }
}
