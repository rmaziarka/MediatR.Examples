namespace KnightFrank.Antares.Dal.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Seed.Common;

    internal class LocaleData
    {
        public static void Seed(KnightFrankContext context)
        {
            List<Locale> locales = Enum.GetNames(typeof(LocaleIsoCode)).Select(x => new Locale { IsoCode = x }).ToList();

            SeedData(locales, context);
        }

        private static void SeedData(List<Locale> locales, KnightFrankContext context)
        {
            locales.ForEach(locale => context.Locale.AddOrUpdate(x => x.IsoCode, locale));
            context.SaveChanges();
        }
    }
}
