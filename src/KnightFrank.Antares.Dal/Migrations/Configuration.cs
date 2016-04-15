namespace KnightFrank.Antares.Dal.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<KnightFrank.Antares.Dal.KnightFrankContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
        }
    }
}
