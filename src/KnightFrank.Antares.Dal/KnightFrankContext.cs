namespace KnightFrank.Antares.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Configuration;
    using KnightFrank.Antares.Dal.Model.Property;

    public class KnightFrankContext : DbContext
    {
        public KnightFrankContext() : base("Api.Settings.SqlConnectionString")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            this.LoadConfigurations(modelBuilder);
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
        public DbSet<CountryLocalised> CountryLocalised { get; set; }
        public DbSet<Property> Property { get; set; }
        public DbSet<Ownership> Ownerships { get; set; }
        public DbSet<Business> Business { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Activity> Activity { get; set; }

        private void LoadConfigurations(DbModelBuilder modelBuilder)
        {
            IEnumerable<Type> mapTypes = from t in typeof(KnightFrankContext).Assembly.GetTypes()
                           where t.BaseType != null 
                           && t.BaseType.IsGenericType 
                           && t.BaseType.GetGenericTypeDefinition() == typeof(BaseEntityConfiguration<>)
                           select t;

            foreach (Type mapType in mapTypes)
            {
                dynamic mapInstance = Activator.CreateInstance(mapType);
                modelBuilder.Configurations.Add(mapInstance);
            }
        }

    }
}