namespace KnightFrank.Antares.Domain.User.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.User;

    public class UserCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<UpdateUserCommand, User>();
        }
    }
}
