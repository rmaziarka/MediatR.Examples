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

            this.HasMany(p => p.Attachments)
                .WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("RequirementId");
                    cs.MapRightKey("AttachmentId");
                });

            this.HasRequired(x => x.RequirementType);
            
            this.HasOptional(p => p.Solicitor)
                .WithMany()
                .HasForeignKey(p => p.SolicitorId)
                .WillCascadeOnDelete(false);

            this.HasOptional(p => p.SolicitorCompany)
                .WithMany()
                .HasForeignKey(p => p.SolicitorCompanyId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Tenancy).WithMany().HasForeignKey(x => x.TenancyId);
        }
    }
}