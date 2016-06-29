namespace KnightFrank.Antares.Dal.Model.Configuration.Offer
{
    using KnightFrank.Antares.Dal.Model.Offer;

    internal sealed class OfferTypeLocalisedConfiguration : BaseEntityConfiguration<OfferTypeLocalised>
    {
        public OfferTypeLocalisedConfiguration()
        {
            this.HasRequired(x => x.Locale).WithMany().HasForeignKey(x => x.LocaleId).WillCascadeOnDelete(false);

            this.Property(c => c.ResourceId).HasColumnName("OfferTypeId");

            this.HasRequired(x => x.OfferType).WithMany().HasForeignKey(x => x.ResourceId).WillCascadeOnDelete(false);

            this.Property(x => x.Value).HasMaxLength(100).IsRequired();
        }
    }
}
