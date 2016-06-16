namespace KnightFrank.Antares.Domain.Common.BusinessValidators
{
    using System;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Specifications;

    public class EnumTypeItemValidator : IEnumTypeItemValidator
    {
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemGenericRepository;

        public EnumTypeItemValidator(IGenericRepository<EnumTypeItem> enumTypeItemGenericRepository)
        {
            this.enumTypeItemGenericRepository = enumTypeItemGenericRepository;
        }

        public void ItemExists(Enums.EnumType enumType, Guid enumTypeItemId)
        {
            bool enumTypeItemExists =
                this.enumTypeItemGenericRepository.Any((new HasId<EnumTypeItem>(enumTypeItemId) & new EnumTypeItemWithEnumType(enumType)).SatisfiedBy());

            if (!enumTypeItemExists)
            {
                throw new BusinessValidationException(BusinessValidationMessage.CreateEnumTypeItemNotExistMessage(enumType.ToString(), enumTypeItemId));
            }
        }

        public void ValidateMandatoryIfItemExists(Enums.EnumType enumType, Guid? enumTypeItemId)
        {
            if (enumTypeItemId.HasValue)
            {
                this.ItemExists(enumType, enumTypeItemId.Value);
            }
            else
            {
                throw new BusinessValidationException(new BusinessValidationMessage(ErrorMessage.Enum_Value_Not_Passed, new[] {enumType.ToString()}));
            }
        }

        public void ItemExists<T>(T enumTypeItem, Guid enumTypeItemId)
        {
            if (enumTypeItem == null)
            {
                throw new BusinessValidationException(BusinessValidationMessage.CreateEnumTypeItemNotExistMessage(typeof(T).ToString(), enumTypeItemId));
            }
        }
    }
}