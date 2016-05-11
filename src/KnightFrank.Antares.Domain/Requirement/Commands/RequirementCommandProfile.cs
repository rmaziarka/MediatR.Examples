namespace KnightFrank.Antares.Domain.Requirement.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.Common.Commands;

    public class RequirementCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateRequirementCommand, Requirement>();

            this.CreateMap<CreateOrUpdateAddress, Address>();
        }
    }
}