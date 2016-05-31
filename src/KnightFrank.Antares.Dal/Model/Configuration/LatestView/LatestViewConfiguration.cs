namespace KnightFrank.Antares.Dal.Model.Configuration.LatestView
{
    using KnightFrank.Antares.Dal.Model.LatestView;

    internal sealed class LatestViewConfiguration : BaseEntityConfiguration<LatestView>
    {
        public LatestViewConfiguration()
        {
            this.HasRequired(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}
