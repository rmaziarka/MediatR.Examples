namespace KnightFrank.Antares.Dal.Seed
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Model.User;

    internal static class UserData
    {
        public static void Seed(KnightFrankContext context)
        {
            SeedUser(context);
        }

        public static void SeedUser(KnightFrankContext context)
        {
            SeedUserData(context, "System Administrator", "John", "Smith", "Residential", "Abode", "GB", "en");
        }

        private static void SeedUserData(KnightFrankContext context, string roleName, string firstName, string lastname, string departmentName, string businessName, string countryCode, string localeCode)
        {
            ICollection<Role> roles = context.Roles.ToList();
            ICollection<Country> countries = context.Countries.ToList();
            ICollection<Business> businesses = context.Businesses.ToList();
            ICollection<Department> departments = context.Departments.ToList();
            ICollection<Locale> locales = context.Locales.ToList();

            var user = new User
            {
                FirstName = firstName,
                LastName = lastname,
                CountryId = GetCountryIdByCode(countries, countryCode),
                BusinessId = GetBusinessIdByCode(businesses, businessName),
                DepartmentId = GetDepartmentIdByCode(departments, departmentName),
                LocaleId = GetLocaleIdByCode(locales, localeCode),
                Roles = new List<Role> { GetRoleByCode(roles, roleName) }
            };

            context.Users.AddOrUpdate(x => new { x.FirstName, x.LastName }, user);
            context.SaveChanges();
        }

        private static Role GetRoleByCode(IEnumerable<Role> roles, string rolename)
        {
            Role role = roles.FirstOrDefault(r => r.Name == rolename);

            return role;
        }

        private static Guid GetCountryIdByCode(IEnumerable<Country> countries, string countryCode)
        {
            Country country = countries.FirstOrDefault(c => c.IsoCode == countryCode);

            return country?.Id ?? default(Guid);
        }

        private static Guid GetBusinessIdByCode(IEnumerable<Business> businesses, string businessName)
        {
            Business business = businesses.FirstOrDefault(b => b.Name == businessName);

            return business?.Id ?? default(Guid);
        }

        private static Guid GetDepartmentIdByCode(IEnumerable<Department> departments, string departmentName)
        {
            Department department = departments.FirstOrDefault(d => d.Name == departmentName);

            return department?.Id ?? default(Guid);
        }

        private static Guid GetLocaleIdByCode(IEnumerable<Locale> locales, string localeCode)
        {
            Locale locale = locales.FirstOrDefault(l => l.IsoCode == localeCode);

            return locale?.Id ?? default(Guid);
        }
    }
}
