namespace KnightFrank.Antares.Dal.Model.Configuration.Offer
{
    using KnightFrank.Antares.Dal.Model.Offer;

    internal sealed class ChainTransactionConfiguration : BaseEntityConfiguration<ChainTransaction>
    {
        public ChainTransactionConfiguration()
        {

            this.HasRequired(x => x.Property)
                .WithMany()
                .HasForeignKey(x => x.PropertyId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Property)
                .WithMany()
                .HasForeignKey(x => x.PropertyId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.AgentContact)
                .WithMany()
                .HasForeignKey(x => x.AgentContactId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.AgentUser)
                .WithMany()
                .HasForeignKey(x => x.AgentUserId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.AgentCompany)
                .WithMany()
                .HasForeignKey(x => x.AgentCompanyId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.SolicitorContact)
                .WithMany()
                .HasForeignKey(x => x.SolicitorContactId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.SolicitorCompany)
                .WithMany()
                .HasForeignKey(x => x.SolicitorCompanyId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Mortgage)
                .WithMany()
                .HasForeignKey(x => x.MortgageId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Survey)
                .WithMany()
                .HasForeignKey(x => x.SurveyId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Searches)
                .WithMany()
                .HasForeignKey(x => x.SearchesId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Enqueries)
                .WithMany()
                .HasForeignKey(x => x.EnqueriesId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.ContactAgreed)
                .WithMany()
                .HasForeignKey(x => x.ContactAgreedId)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Vendor)
                .IsOptional();

            this.Property(x => x.SurveyDate)
                .IsOptional();
        }
    }
}
