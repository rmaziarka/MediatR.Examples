namespace KnightFrank.Antares.Dal.Model
{
    using System;

    using KnightFrank.Antares.Dal.Model.Common;

    public abstract class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }
    }
}
