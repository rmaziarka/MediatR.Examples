namespace KnightFrank.Antares.Dal.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<KnightFrankContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = false;
        }
    }
}
