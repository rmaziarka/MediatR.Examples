namespace KnightFrank.Antares.Domain.Attachment.Commands
{
    using System;

    public class CreateAttachment
    {
        public string FileName { get; set; }

        public long Size { get; set; }

        public Guid DocumentTypeId { get; set; }

        public Guid ExternalDocumentId { get; set; }
    }
}