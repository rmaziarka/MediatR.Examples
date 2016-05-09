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

        /// <summary>
        /// Check if entity with given id exist in database and if not throw exception.
        /// To check method is used Any() therefore entity is not retrieved from the database.
        /// </summary>
        /// <typeparam name="T">Database entity.</typeparam>
        /// <param name="entityId">The entity identifier.</param>
        /// <exception cref="BusinessValidationException"></exception>
        public void EntityExists<T>(Guid entityId) where T : BaseEntity
        {
            IGenericRepository<T> genericRepository = this.GetEntityRepository<T>();

            if (genericRepository.Any(e => e.Id == entityId) == false)
            {
                throw this.GetNotExistException<T>(entityId);
            }
        }

        /// <summary>
        /// Returns the entity if exist in database and throw exception if entity not exist.
        /// To check method is used GetById().
        /// </summary>
        /// <typeparam name="T">Database entity.</typeparam>
        /// <param name="entity">Entity to validate.</param>
        /// <param name="entityId">Entity id.</param>
        /// <returns>Entity instance from database.</returns>
        public void EntityExists<T>(T entity, Guid entityId) where T : BaseEntity
        {
            if (entity == null)
            {
                throw this.GetNotExistException<T>(entityId);
            }
        }

        private BusinessValidationException GetNotExistException<T>(Guid entityId)
        {
            string entityName = typeof(T).Name;
            BusinessValidationMessage buissnessMessage = BusinessValidationMessage.CreateEntityNotExistMessage(entityName, entityId);
            return new BusinessValidationException(buissnessMessage);
        }

        private IGenericRepository<T> GetEntityRepository<T>() where T : BaseEntity
        {
            return this.kernel.Get<IGenericRepository<T>>();
        }
    }
}