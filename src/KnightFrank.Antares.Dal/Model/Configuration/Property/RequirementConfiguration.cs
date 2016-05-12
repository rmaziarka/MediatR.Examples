namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Property;

    internal sealed class RequirementConfiguration : BaseEntityConfiguration<Requirement>
    {
        public RequirementConfiguration()
        {
            this.HasMany(r => r.Contacts)
                .WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("RequirementId");
                    cs.MapRightKey("ContactId");
                });

            this.HasRequired(p => p.Address).WithMany().HasForeignKey(p => p.AddressId).WillCascadeOnDelete(false);

            this.Property(r => r.CreateDate)
                .IsRequired();

            this.Property(r => r.Description)
                .HasMaxLength(4000);

            this.Property(r => r.MinPrice)
                .IsMoney();

            this.Property(r => r.MaxPrice)
                .IsMoney();
        }
    }
}