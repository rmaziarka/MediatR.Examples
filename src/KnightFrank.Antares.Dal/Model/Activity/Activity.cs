namespace KnightFrank.Antares.Dal.Model.Activity
{
    using System;

    using KnightFrank.Antares.Dal.Model.Common;

    public class Activity : BaseEntity, IAuditableEntity
    {
        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
}