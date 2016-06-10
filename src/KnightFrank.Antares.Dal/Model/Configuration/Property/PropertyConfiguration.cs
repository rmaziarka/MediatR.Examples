namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    internal class PropertyConfiguration : BaseEntityConfiguration<Model.Property.Property>
    {
        public PropertyConfiguration()
        {
            this.HasRequired(p => p.Address).WithMany().HasForeignKey(p => p.AddressId);

            this.HasRequired(p => p.PropertyType);
            this.HasOptional(p => p.AttributeValues).WithMany().HasForeignKey(p => p.AttributeValuesId);

            this.HasRequired(p => p.Division)
                .WithMany()
                .HasForeignKey(p => p.DivisionId)
                .WillCascadeOnDelete(false);

            this.Property(x => x.TotalAreaBreakdown)
                .IsOptional();

            this.HasMany(p => p.Attachments)
                .WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("PropertyId");
                    cs.MapRightKey("AttachmentId");
                });
        }
    }
}
