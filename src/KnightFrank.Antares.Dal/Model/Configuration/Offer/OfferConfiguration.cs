namespace KnightFrank.Antares.Dal.Model.Configuration.Offer
{
    using KnightFrank.Antares.Dal.Model.Offer;

    internal sealed class OfferConfiguration : BaseEntityConfiguration<Offer>
    {
        public OfferConfiguration()
        {
            this.HasRequired(x => x.OfferType)
                .WithMany()
                .HasForeignKey(x => x.OfferTypeId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.StatusId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.MortgageStatus)
                .WithMany()
                .HasForeignKey(x => x.MortgageStatusId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.MortgageSurveyStatus)
                .WithMany()
                .HasForeignKey(x => x.MortgageSurveyStatusId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.AdditionalSurveyStatus)
                .WithMany()
                .HasForeignKey(x => x.AdditionalSurveyStatusId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.SearchStatus)
                .WithMany()
                .HasForeignKey(x => x.SearchStatusId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Enquiries)
                .WithMany()
                .HasForeignKey(x => x.EnquiriesId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Requirement)
                .WithMany(x => x.Offers)
                .HasForeignKey(x => x.RequirementId)
                .WillCascadeOnDelete(false);

            this.HasRequired(p => p.Activity)
                .WithMany(r => r.Offers)
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(false);

            this.HasRequired(p => p.Negotiator)
                .WithMany()
                .HasForeignKey(p => p.NegotiatorId)
                .WillCascadeOnDelete(false);

            this.HasOptional(p => p.Broker)
                .WithMany()
                .HasForeignKey(p => p.BrokerId)
                .WillCascadeOnDelete(false);

            this.HasOptional(p => p.BrokerCompany)
                .WithMany()
                .HasForeignKey(p => p.BrokerCompanyId)
                .WillCascadeOnDelete(false);

            this.HasOptional(p => p.Lender)
                .WithMany()
                .HasForeignKey(p => p.LenderId)
                .WillCascadeOnDelete(false);

            this.HasOptional(p => p.LenderCompany)
                .WithMany()
                .HasForeignKey(p => p.LenderCompanyId)
                .WillCascadeOnDelete(false);

            this.HasOptional(p => p.Surveyor)
                .WithMany()
                .HasForeignKey(p => p.SurveyorId)
                .WillCascadeOnDelete(false);

            this.HasOptional(p => p.SurveyorCompany)
                .WithMany()
                .HasForeignKey(p => p.SurveyorCompanyId)
                .WillCascadeOnDelete(false);

            this.HasOptional(p => p.AdditionalSurveyor)
                .WithMany()
                .HasForeignKey(p => p.AdditionalSurveyorId)
                .WillCascadeOnDelete(false);

            this.HasOptional(p => p.AdditionalSurveyorCompany)
                .WithMany()
                .HasForeignKey(p => p.AdditionalSurveyorCompanyId)
                .WillCascadeOnDelete(false);

            this.Property(x => x.SpecialConditions).HasMaxLength(4000);

            this.Property(x => x.CompletionDate)
                .IsOptional();

            this.Property(x => x.ExchangeDate)
                .IsOptional();

            this.Property(o => o.Price)
                .IsMoney()
                .IsOptional();

            this.Property(o => o.PricePerWeek)
                .IsMoney()
                .IsOptional();

            this.Property(x => x.OfferDate)
                .IsRequired();
        }
    }
}
