namespace KnightFrank.Antares.Domain.Requirement.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Property;

    public class RequirementCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateRequirementCommand, Requirement>();

            this.CreateMap<CreateOrUpdateRequirementAddress, Address>();
        }
    }
}