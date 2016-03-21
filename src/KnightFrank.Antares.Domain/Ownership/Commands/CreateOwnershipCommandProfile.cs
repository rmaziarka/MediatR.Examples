namespace KnightFrank.Antares.Domain.Ownership.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;

    public class CreateOwnershipCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateOwnershipCommand, Ownership>();
        }
    }
}
