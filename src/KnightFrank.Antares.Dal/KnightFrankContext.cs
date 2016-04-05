namespace KnightFrank.Antares.Dal
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Attributes.FormDefinition;
    using KnightFrank.Antares.Dal.Model.Configuration;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Model.User;

    public class KnightFrankContext : DbContext
    {
        public KnightFrankContext() : base("Api.Settings.SqlConnectionString")
        {
            this.Database.Initialize(false);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Properties<DateTime>()
                        .Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Entity<PropertyFormDefinition>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("PropertyFormDefinition");
            });

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
        public DbSet<PropertyType> PropertyType { get; set; }
        public DbSet<PropertyTypeLocalised> PropertyTypeLocalised { get; set; }
        public DbSet<PropertyTypeDefinition> PropertyTypeDefinition { get; set; }
        public DbSet<FormDefinition> PropertyFormDefinition { get; set; }

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