namespace KnightFrank.Antares.Domain.Company.CustomValidators
{
    using System;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    public class CompanyCommandCustomValidator : ICompanyCommandCustomValidator
    {
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IEntityValidator entityValidator;

        public CompanyCommandCustomValidator(IEnumTypeItemValidator enumTypeItemValidator, IEntityValidator entityValidator)
        {
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.entityValidator = entityValidator;
        }

        public bool IsClientCareEnumValid(Guid? enumItemId)
        {
            return this.IsEnumValid(EnumType.ClientCareStatus, enumItemId);
        }

        public bool IsCompanyCategoryEnumValid(Guid? enumItemId)
        {
            return this.IsEnumValid(EnumType.CompanyCategory, enumItemId);
        }

        public bool IsCompanyTypeEnumValid(Guid? enumItemId)
        {
            return this.IsEnumValid(EnumType.CompanyType, enumItemId);
        }

        public bool IsRelationshipManagerValid(Guid? entityItemId)
        {
            return this.IsEntityValid<User>(entityItemId);
        }

        private bool IsEnumValid(EnumType enumType, Guid? enumItemId)
        {
            if (enumItemId != null)
            {
                this.enumTypeItemValidator.ItemExists(enumType, (Guid)enumItemId);
            }

            return true;
        }

        private bool IsEntityValid<T>(Guid? entityItemId) where T : BaseEntity
        {
            if (entityItemId != null)
            {
                this.entityValidator.EntityExists<T>((Guid)entityItemId);
            }

            return true;
        }
    }
}
