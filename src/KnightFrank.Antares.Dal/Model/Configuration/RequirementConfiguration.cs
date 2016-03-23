namespace KnightFrank.Antares.Dal.Model.Configuration
{
    using KnightFrank.Antares.Dal.Model;

    internal sealed class RequirementConfiguration : BaseEntityConfiguration<Requirement>
    {
        public RequirementConfiguration()
        {
            this.HasMany(r => r.Contacts)
                .WithMany(c => c.Requirements)
                .Map(cs =>
                {
                    cs.MapLeftKey("RequirementId");
                    cs.MapRightKey("ContactId");
                });

            this.HasRequired(p => p.Address).WithMany().HasForeignKey(p => p.AddressId);

            this.Property(r => r.CreateDate)
                .IsRequired();

            this.Property(r => r.Description)
                .HasMaxLength(4000);

            this.Property(r => r.MinPrice)
                .HasPrecision(19, 4);

            this.Property(r => r.MaxPrice)
                .HasPrecision(19, 4);
        }
    }
}