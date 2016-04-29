namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;

    using KnightFrank.Antares.Dal.Model;

    public interface IEntityValidator
    {
        void EntityExits<T>(Guid entity) where T : BaseEntity;

        void EntityExits<T>(T entity, Guid entityId) where T : BaseEntity;
    }
}