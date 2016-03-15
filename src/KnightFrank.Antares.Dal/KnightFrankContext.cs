namespace KnightFrank.Antares.Dal
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;

    using KnightFrank.Antares.Dal.Migrations;
    using KnightFrank.Antares.Dal.Model;

    public class KnightFrankContext : DbContext
    {
        public KnightFrankContext() : base("Api.Settings.SqlConnectionString")
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
            modelBuilder.Configurations.Add(new EnumLocalisedConfiguration());
            modelBuilder.Configurations.Add(new LocaleConfiguration());
            modelBuilder.Configurations.Add(new AddressFieldDefinitionConfiguration());
            modelBuilder.Configurations.Add(new AddressFieldConfiguration());
            modelBuilder.Configurations.Add(new AddressFieldLableConfiguration());
            modelBuilder.Configurations.Add(new AddressConfiguration());
            modelBuilder.Configurations.Add(new AddressFormEntityTypeConfiguration());
            modelBuilder.Configurations.Add(new AddressFormConfigutration());
            modelBuilder.Configurations.Add(new CountryConfiguration());
        }

        public DbSet<Contact> Contact { get; set; }
        public DbSet<Requirement> Requirement { get; set; }

        public DbSet<EnumType> EnumType { get; set; }
        public DbSet<EnumTypeItem> EnumTypeItem { get; set; }
        public DbSet<EnumLocalised> EnumLocalised { get; set; }
        public DbSet<Locale> Locale { get; set; }
        public DbSet<AddressFieldDefinition> AddressFieldDefinition { get; set; }
        public DbSet<AddressFieldLabel> AddressFieldLabel { get; set; }
        public DbSet<AddressField> AddressField { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<AddressFormEntityType> AddressFormEntityType { get; set; }
        public DbSet<AddressForm> AddressForm { get; set; }
        public DbSet<Country> Country { get; set; }
    }
}