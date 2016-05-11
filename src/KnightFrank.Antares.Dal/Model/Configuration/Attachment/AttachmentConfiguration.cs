namespace KnightFrank.Antares.Dal.Model.Configuration.Attachment
{
    using KnightFrank.Antares.Dal.Model.Attachment;
    internal sealed class AttachmentConfiguration : BaseEntityConfiguration<Attachment>
    {
        public AttachmentConfiguration()
        {
            this.Property(p => p.FileName).HasMaxLength(255).IsRequired();
            this.Property(p => p.ExternalDocumentId).IsRequired();

            this.HasRequired(p => p.DocumentType).WithMany().WillCascadeOnDelete(false);
        }
    }
}