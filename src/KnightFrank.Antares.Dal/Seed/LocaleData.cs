namespace KnightFrank.Antares.Dal.Seed
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    using KnightFrank.Antares.Dal.Model;

    internal class LocaleData
    {
        public static void Seed(KnightFrankContext context)
        {
            var locales = new List<Locale>
                              {
                                  new Locale { IsoCode = "cs" },
								  new Locale { IsoCode = "de" },
								  new Locale { IsoCode = "en" },
								  new Locale { IsoCode = "es" },
								  new Locale { IsoCode = "fr" },
								  new Locale { IsoCode = "pl" },
								  new Locale { IsoCode = "sv" }
							  };

            SeedData(locales, context);
        }

        private static void SeedData(List<Locale> locales, KnightFrankContext context)
        {
            locales.ForEach(locale => context.Locale.AddOrUpdate(x => x.IsoCode, locale));
            context.SaveChanges();
        }
    }
}