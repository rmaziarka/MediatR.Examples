namespace KnightFrank.Antares.Api.Models
{
    using System;

    public class AttachmentUrlParameters
    {
        public string LocaleIsoCode { get; set; }

        public Guid DocumentTypeId { get; set; }

        public Guid EntityReferenceId { get; set; }

        public string Filename { get; set; }
    }
}