namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Property;

    internal sealed class PropertyTypeConfiguration: BaseEntityConfiguration<PropertyType>
    {
        public PropertyTypeConfiguration()
        {
            this.HasOptional(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
