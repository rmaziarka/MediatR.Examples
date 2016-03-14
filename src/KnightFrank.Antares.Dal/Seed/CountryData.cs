namespace KnightFrank.Antares.Dal.Seed
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    using KnightFrank.Antares.Dal.Model;

    internal class CountryData
    {
        public static void Seed(KnightFrankContext context)
        {
            var countires = new List<Country>
                                {
                                    new Country { Code = "uk" }
                                };

            SeedData(countires, context);
        }

        private static void SeedData(List<Country> countires, KnightFrankContext context)
        {
            countires.ForEach(country => context.Country.AddOrUpdate(x => x.Code, country));
            context.SaveChanges();
        }
    }
}