namespace KnightFrank.Antares.Dal.Model.Attachment
{
    using System;

    using KnightFrank.Antares.Dal.Model.Enum;

    public class Attachment : BaseAuditableEntity
    {
        public string FileName { get; set; }
        public Guid DocumentTypeId { get; set; }
        public virtual EnumTypeItem DocumentType { get; set; }
        public long Size { get; set; }
        public Guid ExternalDocumentId { get; set; }
    }
}