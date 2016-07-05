namespace KnightFrank.Antares.Dal.Model.Configuration.Property.Activities
{
    using KnightFrank.Antares.Dal.Model.Property.Activities;

    internal sealed class ActivityConfiguration : BaseEntityConfiguration<Activity>
    {
        public ActivityConfiguration()
        {
            this.HasRequired(a => a.Property)
                .WithMany(p => p.Activities)
                .HasForeignKey(a => a.PropertyId)
                .WillCascadeOnDelete(false);

            this.HasRequired(a => a.ActivityStatus).WithMany().HasForeignKey(s => s.ActivityStatusId).WillCascadeOnDelete(false);

            this.HasRequired(a => a.ActivityType).WithMany().HasForeignKey(s => s.ActivityTypeId).WillCascadeOnDelete(false);
            
            this.HasMany(a => a.Contacts).WithMany().Map(
                cs =>
                    {
                        cs.MapLeftKey("ActivityId");
                        cs.MapRightKey("ContactId");
                    });


            this.Property(o => o.MarketAppraisalPrice)
                .IsMoney();

            this.Property(o => o.RecommendedPrice)
                .IsMoney();

            this.Property(o => o.VendorEstimatedPrice)
                .IsMoney();

            this.Property(o => o.ShortLetPricePerWeek)
                .IsMoney();

            this.Property(o => o.AskingPrice)
                .IsMoney();

            this.HasMany(p => p.Attachments)
                .WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("ActivityId");
                    cs.MapRightKey("AttachmentId");
                });

            this.HasOptional(p => p.Solicitor)
                .WithMany()
                .HasForeignKey(p => p.SolicitorId)
                .WillCascadeOnDelete(false);

            this.HasOptional(p => p.SolicitorCompany)
                .WithMany()
                .HasForeignKey(p => p.SolicitorCompanyId)
                .WillCascadeOnDelete(false);

            this.HasOptional(a => a.Source).WithMany().HasForeignKey(s => s.SourceId).WillCascadeOnDelete(false);
            
            this.HasOptional(a => a.SellingReason).WithMany().HasForeignKey(s => s.SellingReasonId).WillCascadeOnDelete(false);

            this.Property(a => a.SourceDescription)
                .HasMaxLength(4000);

            this.Property(a => a.PitchingThreats)
                .HasMaxLength(4000);

            this.Property(a => a.KeyNumber)
                .HasMaxLength(128);

            this.Property(a => a.AccessArrangements)
                .HasMaxLength(4000);

            this.Property(a => a.AppraisalMeetingStart);

            this.Property(a => a.AppraisalMeetingEnd);

            this.Property(a => a.AppraisalMeetingInvitationText)
                .HasMaxLength(4000);
        }
    }
}
