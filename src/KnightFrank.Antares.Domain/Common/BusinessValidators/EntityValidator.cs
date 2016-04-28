namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;

    using Ninject;

    public class EntityValidator : IEntityValidator
    {
        private readonly IKernel kernel;

        public EntityValidator(IKernel kernel)
        {
            this.kernel = kernel;
        }
        public void ThrowExceptionIfNotExist<T>(Guid entityId) where T : BaseEntity
        {
            var genericRepository = this.kernel.Get<IGenericRepository<T>>();

            if (genericRepository.Any(e => e.Id == entityId) == false)
            {
                string entityName = typeof(T).Name;
                BusinessValidationMessage buissnessMessage = BusinessValidationMessage.CreateEntityNotExistMessage(entityName, entityId);
                throw new BusinessValidationException(buissnessMessage);
            };
        }
    }
}