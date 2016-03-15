namespace KnightFrank.Antares.Dal.Seed
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    using KnightFrank.Antares.Dal.Model;

    internal class CountryData
    {
        public static void Seed(KnightFrankContext context)
        {
            var countries = new List<Country>
                                {
                                    new Country { Code = "uk" }
                                };

            SeedData(countries, context);
        }

        private static void SeedData(List<Country> countries, KnightFrankContext context)
        {
            countries.ForEach(country => context.Country.AddOrUpdate(x => x.Code, country));
            context.SaveChanges();
        }
    }
}