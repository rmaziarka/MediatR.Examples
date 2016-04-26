namespace KnightFrank.Antares.Domain.Common.BuissnessValidators
{
    using System;

    using KnightFrank.Antares.Dal.Model;

    public interface IEntityValidator
    {
        void ThrowExceptionIfNotExist<T>(Guid entity) where T : BaseEntity;
    }
}