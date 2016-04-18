namespace KnightFrank.Antares.Dal.Model.Configuration.Property
{
    using KnightFrank.Antares.Dal.Model.Property;

    internal sealed class RequirementNoteConfiguration : BaseEntityConfiguration<RequirementNote>
    {
        public RequirementNoteConfiguration()
        {
            this.HasRequired(n => n.Requirement)
                .WithMany(r => r.RequirementNotes)
                .HasForeignKey(n => n.RequirementId)
                .WillCascadeOnDelete(false);

            this.Property(n => n.Description)
                .HasMaxLength(4000);
        }
    }
}
