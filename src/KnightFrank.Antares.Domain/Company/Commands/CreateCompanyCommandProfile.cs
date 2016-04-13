namespace KnightFrank.Antares.Domain.Company.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Company;

    public class CreateCompanyCommandProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateCompanyCommand, Company>();
        }
    }
}