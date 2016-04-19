namespace KnightFrank.Antares.Dal.Model.Configuration.Attribute
{
    using KnightFrank.Antares.Dal.Model.Attribute;

    internal class AttributeConfiguration : BaseEntityConfiguration<Attribute>
    {
        public AttributeConfiguration()
        {
            this.Property(p => p.NameKey).HasMaxLength(100).IsRequired().IsUnique();
            this.Property(p => p.LabelKey).HasMaxLength(null);
        }
    }
}
