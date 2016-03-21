namespace KnightFrank.Antares.Domain.UnitTests.FixtureExtension
{
    using KnightFrank.Antares.Dal.Model;

    using Ploeh.AutoFixture;

    public static class CountryFixtureExtensions
    {
        public static CountryLocalised BuildCountryLocalised(this IFixture fixture, string isoCode, string localeValue = null)
        {
            if (string.IsNullOrEmpty(localeValue))
            {
                localeValue = fixture.Create<string>();
            }

            Locale locale = fixture.Build<Locale>()
                                   .With(x => x.IsoCode, isoCode)
                                   .Create();

            return fixture.Build<CountryLocalised>()
                   .With(x => x.Locale, locale)
                   .With(x => x.Value, localeValue)
                   .Create();
        }
    }
}