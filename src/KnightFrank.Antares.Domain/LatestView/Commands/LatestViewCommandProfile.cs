namespace KnightFrank.Antares.Domain.LatestView.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.LatestView;

    public class LatestViewCommandProfile:Profile
    {
       protected override void Configure()
       {
           this.CreateMap<CreateLatestViewCommand, LatestView>();
       }
    }
}
