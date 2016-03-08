namespace KnightFrank.Antares.Dal
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    using KnightFrank.Antares.Dal.Migrations;
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

            modelBuilder.Configurations.Add(new ContactConfiguration());
            modelBuilder.Configurations.Add(new RequirementConfiguration());
            modelBuilder.Configurations.Add(new EnumTypeConfiguration());
            modelBuilder.Configurations.Add(new EnumTypeItemConfiguration());
            modelBuilder.Configurations.Add(new EnumLocalisationConfiguration());
            modelBuilder.Configurations.Add(new LocalConfiguration());
        }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<Requirement> Requirement { get; set; }

        public DbSet<EnumType> EnumType { get; set; }
        public DbSet<EnumTypeItem> EnumTypeItem { get; set; }
        public DbSet<EnumLocalisation> EnumLocalisation { get; set; }
        public DbSet<Local> Local { get; set; }

    }
}