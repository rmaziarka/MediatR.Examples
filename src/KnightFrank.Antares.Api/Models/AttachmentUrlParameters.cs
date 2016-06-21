namespace KnightFrank.Antares.Api.Models
{
    using System;

    using KnightFrank.Foundation.Antares.Cloud.Storage.Blob;

    public class AttachmentUrlParameters
    {
        public string LocaleIsoCode { get; set; }

        public Guid DocumentTypeId { get; set; }

        public Guid EntityReferenceId { get; set; }

        public string Filename { get; set; }

        public CloudStorageContainerType cloudStorageContainerType;
    }
}