namespace KnightFrank.Antares.Dal.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<KnightFrankContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(KnightFrankContext context)
        {
            context.Contacts.AddOrUpdate(
                new Contact { Surname = "Andrew Peters" }, 
                new Contact { Surname = "Brice Lambson" }, 
                new Contact { Surname = "Rowan Miller" });
        }
    }
}
