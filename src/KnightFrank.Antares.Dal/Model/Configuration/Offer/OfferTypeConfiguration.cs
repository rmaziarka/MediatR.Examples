namespace KnightFrank.Antares.Dal.Model.Configuration.Offer
{
    using KnightFrank.Antares.Dal.Model.Offer;

    internal sealed class OfferTypeConfiguration : BaseEntityConfiguration<OfferType>
    {
        public OfferTypeConfiguration()
        {
            this.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            this.Property(x => x.EnumCode)
                .HasMaxLength(250)
                .IsRequired()
                .IsUnique();
        }
    }
}
