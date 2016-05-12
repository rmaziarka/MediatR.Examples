namespace KnightFrank.Antares.Dal.Model.Configuration.Offer
{
    using KnightFrank.Antares.Dal.Model.Offer;

    internal sealed class OfferConfiguration : BaseEntityConfiguration<Offer>
    {
        public OfferConfiguration()
        {
            this.HasRequired(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.StatusId)
                .WillCascadeOnDelete(false);

            this.HasRequired(x => x.Requirement)
                .WithMany(x => x.Offers)
                .HasForeignKey(x => x.RequirementId)
                .WillCascadeOnDelete(false);

            this.HasRequired(p => p.Activity)
                .WithMany()
                .HasForeignKey(p => p.ActivityId)
                .WillCascadeOnDelete(false);

            this.HasRequired(p => p.Negotiator)
                .WithMany()
                .HasForeignKey(p => p.NegotiatorId)
                .WillCascadeOnDelete(false);

            this.Property(x => x.SpecialConditions).HasMaxLength(4000);

            this.Property(x => x.CompletionDate)
                .IsOptional();

            this.Property(x => x.ExchangeDate)
                .IsOptional();

            this.Property(o => o.Price)
                .HasPrecision(19, 4)
                .IsRequired();

            this.Property(x => x.OfferDate)
                .IsRequired();
        }
    }
}
