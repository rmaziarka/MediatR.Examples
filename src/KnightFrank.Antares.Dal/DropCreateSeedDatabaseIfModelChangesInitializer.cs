namespace KnightFrank.Antares.Dal
{
    using System.Data.Entity;

    using KnightFrank.Antares.Dal.Seed;

    public class DropCreateSeedDatabaseIfModelChangesInitializer : DropCreateDatabaseIfModelChanges<KnightFrankContext>
    {
        protected override void Seed(KnightFrankContext context)
        {
            DatabaseSeeder.Seed(context);
        }
    }
}
