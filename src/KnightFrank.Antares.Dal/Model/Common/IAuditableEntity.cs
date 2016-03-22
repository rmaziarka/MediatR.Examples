namespace KnightFrank.Antares.Dal.Model.Common
{
    using System;

    public interface IAuditableEntity
    {
         DateTime CreatedAt { get; set; }

         DateTime LastModifiedAt { get; set; }
    }
}