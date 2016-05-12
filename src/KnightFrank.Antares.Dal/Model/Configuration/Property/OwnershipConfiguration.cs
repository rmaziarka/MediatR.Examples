namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Property;
    internal sealed class OwnershipConfiguration : BaseEntityConfiguration<Ownership>
    {
        public OwnershipConfiguration()
        {
            this.HasRequired(o => o.Property);

            this.HasMany(o => o.Contacts)
                .WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("OwnershipId");
                    cs.MapRightKey("ContactId");
                });

            this.Property(o => o.BuyPrice)
                .IsMoney();

            this.Property(o => o.SellPrice)
                .IsMoney();
        }

    }
}
