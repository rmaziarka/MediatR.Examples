namespace KnightFrank.Antares.Domain.Common.Enums
{
    using System;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    public class EnumParser : IEnumParser
    {
        private readonly INinjectInstanceResolver ninjectInstanceResolver;

        public EnumParser(INinjectInstanceResolver ninjectInstanceResolver)
        {
            this.ninjectInstanceResolver = ninjectInstanceResolver;
        }

        public TResult Parse<T, TResult>(Guid enumEntityId)
            where T : BaseEntityWithCode
            where TResult : struct
        {
            IGenericRepository<T> genericRepository = this.ninjectInstanceResolver.GetEntityGenericRepository<T>();
            T enumEntity = genericRepository.GetById(enumEntityId);
            if (enumEntity != null)
            {
                TResult enumValue;
                if (Enum.TryParse(enumEntity.EnumCode, out enumValue))
                {
                    return enumValue;
                }
            }

           throw new BusinessValidationException(BusinessValidationMessage.CreateEnumTypeItemNotExistMessage(typeof(T).ToString(), enumEntityId));
        }
    }
}
