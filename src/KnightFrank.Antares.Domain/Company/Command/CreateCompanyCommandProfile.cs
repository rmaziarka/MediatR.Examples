namespace KnightFrank.Antares.Domain.Company.Command
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