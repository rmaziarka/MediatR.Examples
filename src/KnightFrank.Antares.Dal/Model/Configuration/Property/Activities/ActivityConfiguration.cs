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

            this.Property(o => o.KfValuationPrice)
                .IsMoney();

            this.Property(o => o.VendorValuationPrice)
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

            this.Property(a => a.ServiceChargeAmount).IsMoney();
            this.Property(a => a.ServiceChargeNote).HasMaxLength(4000);
            this.Property(a => a.GroundRentAmount).IsMoney();
            this.Property(a => a.GroundRentNote).HasMaxLength(4000);

            this.HasOptional(a => a.DisposalType).WithMany().HasForeignKey(s => s.DisposalTypeId).WillCascadeOnDelete(false);

            this.Property(a => a.OtherCondition).HasMaxLength(4000);

            this.HasOptional(a => a.Decoration).WithMany().HasForeignKey(s => s.DecorationId).WillCascadeOnDelete(false);

            this.Property(a => a.ShortKfValuationPrice).IsMoney();
            this.Property(a => a.ShortVendorValuationPrice).IsMoney();
            this.Property(a => a.ShortAgreedInitialMarketingPrice).IsMoney();

            this.Property(a => a.LongKfValuationPrice).IsMoney();
            this.Property(a => a.LongVendorValuationPrice).IsMoney();
            this.Property(a => a.LongAgreedInitialMarketingPrice).IsMoney();
            
            this.HasOptional(a => a.PriceType).WithMany().HasForeignKey(s => s.PriceTypeId).WillCascadeOnDelete(false);
            this.HasOptional(a => a.MatchFlexibility).WithMany().HasForeignKey(s => s.MatchFlexibilityId).WillCascadeOnDelete(false);
            this.HasOptional(a => a.ShortMatchFlexibility).WithMany().HasForeignKey(s => s.ShortMatchFlexibilityId).WillCascadeOnDelete(false);
            this.HasOptional(a => a.LongMatchFlexibility).WithMany().HasForeignKey(s => s.LongMatchFlexibilityId).WillCascadeOnDelete(false);

            this.Property(a => a.ActivityPrice).IsMoney();
            this.Property(a => a.MatchFlexValue).IsMoney();
            this.Property(a => a.ShortAskingWeekRent).IsMoney();
            this.Property(a => a.ShortAskingMonthRent).IsMoney();
            this.Property(a => a.LongAskingWeekRent).IsMoney();
            this.Property(a => a.LongAskingMonthRent).IsMoney();
            this.Property(a => a.ShortMatchFlexWeekValue).IsMoney();
            this.Property(a => a.ShortMatchFlexMonthValue).IsMoney();
            this.Property(a => a.LongMatchFlexWeekValue).IsMoney();
            this.Property(a => a.LongMatchFlexMonthValue).IsMoney();
        }
    }
}
