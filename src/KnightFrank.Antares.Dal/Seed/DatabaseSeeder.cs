namespace KnightFrank.Antares.Dal.Seed
{
    public class DatabaseSeeder
    {
        public static void Seed(KnightFrankContext context)
        {
            LocaleData.Seed(context);
            CountryData.Seed(context);
            EntityTypeEnumData.Seed(context);

            context.SaveChanges();
        }
    }
}
