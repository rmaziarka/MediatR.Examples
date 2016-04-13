namespace KnightFrank.Antares.Api.IntegrationTests
{
    using System.Data.Entity;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Seed;

    public class DropCreateSeedDatabaseAlwaysInitializer : DropCreateDatabaseAlways<KnightFrankContext>
    {
        protected override void Seed(KnightFrankContext context)
        {
            DatabaseSeeder.Seed(context);
        }
    }
}
