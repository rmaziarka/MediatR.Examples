namespace KnightFrank.Antares.Dal.Model.Configuration.PropertyType
{
    using KnightFrank.Antares.Dal.Model.PropertyType;

    internal sealed class PropertyTypeConfiguration: BaseEntityConfiguration<PropertyType>
    {
        public PropertyTypeConfiguration()
        {
            this.HasOptional(x => x.Parent)
                .WithMany()
                .HasForeignKey(x => x.ParentId)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
