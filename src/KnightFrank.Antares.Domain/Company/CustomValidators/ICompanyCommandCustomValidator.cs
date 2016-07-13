namespace KnightFrank.Antares.Domain.Company.CustomValidators
{
    using System;

    public interface ICompanyCommandCustomValidator
    {
        bool IsClientCareEnumValid(Guid? enumItemId);

        bool IsCompanyCategoryEnumValid(Guid? enumItemId);

        bool IsCompanyTypeEnumValid(Guid? enumItemId);
    }
}
