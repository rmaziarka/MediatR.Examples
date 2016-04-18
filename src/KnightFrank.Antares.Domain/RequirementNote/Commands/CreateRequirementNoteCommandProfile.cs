namespace KnightFrank.Antares.Domain.RequirementNote.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Property;

    public class CreateRequirementNoteCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateRequirementNoteCommand, RequirementNote>();
        }
    }
}
