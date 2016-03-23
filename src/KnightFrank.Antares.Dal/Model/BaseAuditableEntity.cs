namespace KnightFrank.Antares.Dal.Model
{
    using System;

    using KnightFrank.Antares.Dal.Model.Common;

    public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
    {
        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
