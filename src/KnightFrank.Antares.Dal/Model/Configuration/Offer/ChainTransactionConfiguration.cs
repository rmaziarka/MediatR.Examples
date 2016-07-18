namespace KnightFrank.Antares.Dal.Model.Configuration.Offer
{
    using KnightFrank.Antares.Dal.Model.Offer;

    internal sealed class ChainTransactionConfiguration : BaseEntityConfiguration<ChainTransaction>
    {
        public ChainTransactionConfiguration()
        {
            this.HasOptional(x => x.Parent)
                .WithMany()
                .HasForeignKey(x => x.ParentId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Activity)
                .WithMany(x => x.ChainTransactions)
                .HasForeignKey(x => x.ActivityId)
                .WillCascadeOnDelete(false);

            this.HasOptional(x => x.Requirement)
                .WithMany(x => x.ChainTransactions)
                .HasForeignKey(x => x.RequirementId)
                .WillCascadeOnDelete(false);

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

            this.HasRequired(x => x.Enquiries)
                .WithMany()
                .HasForeignKey(x => x.EnquiriesId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.ContractAgreed)
                .WithMany()
                .HasForeignKey(x => x.ContractAgreedId)
                .WillCascadeOnDelete(false);

            this.Property(x => x.Vendor)
                .IsOptional();

            this.Property(x => x.SurveyDate)
                .IsOptional();

            this.Property(x => x.CreatedDate)
                .IsRequired();

            this.Property(x => x.LastModifiedDate)
                .IsRequired();
        }
    }
}
