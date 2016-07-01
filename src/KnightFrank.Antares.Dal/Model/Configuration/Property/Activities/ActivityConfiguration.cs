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


            this.Property(o => o.AgreedInitialMarketingPrice)
                .IsMoney();

            this.Property(o => o.KFValuationPrice)
                .IsMoney();

            this.Property(o => o.VendorValuationPrice)
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

            this.Property(x => x.ServiceChargeAmount).IsMoney();
            this.Property(x => x.ServiceChargeNote).HasMaxLength(4000);
            this.Property(x => x.GroundRentAmount).IsMoney();
            this.Property(x => x.GroundRentNote).HasMaxLength(4000);

            this.HasOptional(a => a.DisposalType).WithMany().HasForeignKey(s => s.DisposalTypeId).WillCascadeOnDelete(false);

            this.Property(x => x.OtherCondition).HasMaxLength(4000);

            this.HasOptional(a => a.Decoration).WithMany().HasForeignKey(s => s.DecorationId).WillCascadeOnDelete(false);

            this.Property(x => x.ShortKFValuationPrice).IsMoney();
            this.Property(x => x.ShortVendorValuationPrice).IsMoney();
            this.Property(x => x.ShortAgreedInitialMarketingPrice).IsMoney();

            this.Property(x => x.LongKFValuationPrice).IsMoney();
            this.Property(x => x.LongVendorValuationPrice).IsMoney();
            this.Property(x => x.LongAgreedInitialMarketingPrice).IsMoney();
        }
    }
}
