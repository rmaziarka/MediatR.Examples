namespace KnightFrank.Antares.Dal.Model.Configuration
{
    using KnightFrank.Antares.Dal.Migrations;

    internal class PropertyConfiguration : BaseEntityConfiguration<Property>
    {
        public PropertyConfiguration()
        {
            this.HasRequired(p => p.Address).WithOptional(a => a.Property);
        }
    }
}
