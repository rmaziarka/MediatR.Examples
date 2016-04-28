namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;

    using KnightFrank.Antares.Dal.Model;

    public interface IEntityValidator
    {
        void ThrowExceptionIfNotExist<T>(Guid entity) where T : BaseEntity;
    }
}