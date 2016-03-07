namespace KnightFrank.Antares.Dal
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using KnightFrank.Antares.Dal.Model;

    public class KnightFrankContext : DbContext
    {
        public KnightFrankContext() : base("KnightFrankConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<Contact> Contact { get; set; }

        public DbSet<Requirement> Requirement { get; set; }
    }
}
