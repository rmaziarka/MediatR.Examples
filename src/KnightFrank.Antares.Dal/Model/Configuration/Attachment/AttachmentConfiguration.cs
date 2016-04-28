namespace KnightFrank.Antares.Dal.Model.Configuration.Attachment
{
    using KnightFrank.Antares.Dal.Model.Attachment;
    internal sealed class AttachmentConfiguration : BaseEntityConfiguration<Attachment>
    {
        public AttachmentConfiguration()
        {
            this.Property(p => p.FileName).HasMaxLength(255).IsRequired();
            this.Property(p => p.AzureDocumentId).IsRequired();

            this.HasRequired(p => p.FileType).WithMany().WillCascadeOnDelete(false);
        }
    }
}