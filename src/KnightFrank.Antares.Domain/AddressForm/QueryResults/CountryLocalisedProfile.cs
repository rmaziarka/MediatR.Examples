namespace KnightFrank.Antares.Domain.AddressForm.QueryResults
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model;

    public class CountryLocalisedProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<Country, CountryResult>();
            this.CreateMap<Locale, LocaleResult>();
            this.CreateMap<CountryLocalised, CountryLocalisedResult>();
        }
    }
}