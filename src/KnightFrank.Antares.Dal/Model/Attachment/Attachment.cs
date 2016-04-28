namespace KnightFrank.Antares.Dal.Model.Attachment
{
    using System;

    using KnightFrank.Antares.Dal.Model.Enum;

    public class Attachment : BaseAuditableEntity
    {
        public string FileName { get; set; }
        public Guid FileTypeId { get; set; }
        public virtual EnumTypeItem FileType { get; set; }
        public long Size { get; set; }
        public Guid AzureDocumentId { get; set; }
    }
}