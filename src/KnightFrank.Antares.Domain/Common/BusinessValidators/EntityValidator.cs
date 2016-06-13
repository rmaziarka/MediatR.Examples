namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;

    public class EntityValidator : IEntityValidator
    {
        private readonly INinjectInstanceResolver ninjectInstanceResolver;

        public EntityValidator(INinjectInstanceResolver ninjectInstanceResolver)
        {
            this.ninjectInstanceResolver = ninjectInstanceResolver;
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
            IGenericRepository<T> genericRepository = this.GetEntityGenericRepository<T>();

            if (genericRepository.Any(e => e.Id == entityId) == false)
            {
                throw this.GetNotExistException<T>(entityId);
            }
        }

        /// <summary>
        /// Check if entity with given id exist in database and if not throw exception.
        /// To check method is used Any() therefore entity is not retrieved from the database.
        /// </summary>
        /// <typeparam name="T">Database entity.</typeparam>
        /// <param name="entityId">The entity identifier.</param>
        /// <exception cref="BusinessValidationException"></exception>
        public void EntityExists<T>(Guid? entityId) where T : BaseEntity
        {
            if (entityId.HasValue)
            {
                this.EntityExists<T>(entityId.Value);
            }
        }

        /// <summary>
        /// Check if entities with given ids exis in database and if not throw exception.
        /// To check method is used Any() therefore entities are not retrieved from the database.
        /// </summary>
        /// <typeparam name="T">Database entity.</typeparam>
        /// <param name="collection">The entity identifiers collection.</param>
        /// <exception cref="BusinessValidationException"></exception>
        public void EntitiesExist<T>(ICollection<Guid> collection) where T : BaseEntity
        {
            IGenericRepository<T> genericRepository = this.GetEntityGenericRepository<T>();

            IEnumerable<Guid> existingEntities = genericRepository.FindBy(x => collection.Contains(x.Id)).Select(x => x.Id).ToList();
            if (!collection.All(x => existingEntities.Contains(x)))
            {
                throw this.GetOneOfEntitiesNotExistException<T>();
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

        private BusinessValidationException GetOneOfEntitiesNotExistException<T>()
        {
            string entityName = typeof(T).Name;
            BusinessValidationMessage buissnessMessage = BusinessValidationMessage.CreateOneOfEntitiesNotExistMessage(entityName);
            return new BusinessValidationException(buissnessMessage);
        }

        private IGenericRepository<T> GetEntityGenericRepository<T>() where T : BaseEntity
        {
            return this.ninjectInstanceResolver.GetEntityGenericRepository<T>();
        }
    }
}