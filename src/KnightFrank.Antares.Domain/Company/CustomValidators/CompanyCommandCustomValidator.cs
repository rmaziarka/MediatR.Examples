namespace KnightFrank.Antares.Domain.Company.CustomValidators
{
    using System;

    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    public class CompanyCommandCustomValidator : ICompanyCommandCustomValidator
    {
        private readonly IEnumTypeItemValidator enumTypeItemValidator;

        public CompanyCommandCustomValidator(IEnumTypeItemValidator enumTypeItemValidator)
        {
            this.enumTypeItemValidator = enumTypeItemValidator;
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

        private bool IsEnumValid(EnumType enumType, Guid? enumItemId)
        {
            if (enumItemId != null)
            {
                this.enumTypeItemValidator.ItemExists(enumType, (Guid)enumItemId);
            }

            return true;
        }
    }
}
