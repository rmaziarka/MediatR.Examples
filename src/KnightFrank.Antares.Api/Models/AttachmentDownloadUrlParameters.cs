namespace KnightFrank.Antares.Api.Models
{
    using System;

    public class AttachmentDownloadUrlParameters : AttachmentUrlParameters
    {
        public Guid ExternalDocumentId { get; set; }
    }
}