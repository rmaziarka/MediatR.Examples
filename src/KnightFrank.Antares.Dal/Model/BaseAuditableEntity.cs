namespace KnightFrank.Antares.Dal.Model
{
    using System;

    using KnightFrank.Antares.Dal.Model.Common;

    public abstract class BaseAuditableEntity : BaseEntity, IAuditableEntity
    {
        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public Guid? UserId { get; set; }

        public virtual User.User User { get; set; }
    }
}
