namespace KnightFrank.Antares.Dal.Migrations
{
    using System.Data.Entity.Migrations;

    using KnightFrank.Antares.Dal.Seed;

    internal sealed class Configuration : DbMigrationsConfiguration<KnightFrankContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(KnightFrankContext context)
        {
            LocaleData.Seed(context);
            CountryData.Seed(context);
            EntityTypeEnumData.Seed(context);
            AddressFormData.Seed(context);

            context.SaveChanges();
        }
    }
}
