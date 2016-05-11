namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Property;

    internal sealed class ViewingConfiguration : BaseEntityConfiguration<Viewing>
    {
        public ViewingConfiguration()
        {
            this.HasMany(r => r.Attendees)
                .WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("ViewingId");
                    cs.MapRightKey("AttendeeId");
                });

            this.Property(r => r.StartDate)
                .IsRequired();

            this.Property(r => r.EndDate)
                .IsRequired();

            this.HasRequired(p => p.Negotiator)
                .WithMany()
                .HasForeignKey(p => p.NegotiatorId);

            this.HasRequired(p => p.Activity)
                .WithMany()
                .HasForeignKey(p => p.ActivityId);

            this.HasRequired(p => p.Requirement)
                .WithMany()
                .HasForeignKey(p => p.RequirementId);

            this.Property(r => r.InvitationText)
                .HasMaxLength(4000);

            this.Property(r => r.PostViewingComment)
                .HasMaxLength(4000);

            this.HasRequired(v => v.Requirement)
                .WithMany(r => r.Viewings);
        }
    }
}
