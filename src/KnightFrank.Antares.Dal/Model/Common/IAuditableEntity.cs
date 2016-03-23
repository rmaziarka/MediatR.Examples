namespace KnightFrank.Antares.Dal.Model.Common
{
    using System;

    public interface IAuditableEntity
    {
         DateTime CreatedDate { get; set; }

         DateTime LastModifiedDate { get; set; }
    }
}