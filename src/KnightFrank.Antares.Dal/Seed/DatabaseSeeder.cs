namespace KnightFrank.Antares.Dal.Seed
{
    public class DatabaseSeeder
    {
        public static void Seed(KnightFrankContext context)
        {
            LocaleData.Seed(context);
            CountryData.Seed(context);
			DepartmentData.Seed(context);
			BusinessData.Seed(context);
            EnumData.Seed(context);
            AddressFormData.Seed(context);
			RoleData.Seed(context);
            PropertyTypeData.Seed(context);
            AttributeData.Seed(context);
            PropertyAttributeFromData.Seed(context);

            context.SaveChanges();
        }
    }
}
