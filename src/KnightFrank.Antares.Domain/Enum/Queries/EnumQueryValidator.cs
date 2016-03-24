namespace KnightFrank.Antares.Domain.Enum.Queries
{
    using System;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;

    public class EnumQueryValidator : AbstractValidator<EnumQuery>
    {
        public EnumQueryValidator(IReadGenericRepository<EnumType> enumTypeRepository)
        {
            Func<EnumQuery, ValidationFailure> enumCodeExists = query =>
                {
                    string propertyName = nameof(query.Code);

                    EnumType enumType = enumTypeRepository.Get().SingleOrDefault(x => x.Code == query.Code);

                    return enumType == null ? new ValidationFailure(propertyName, "Enum does not exists.") : null;
                };

            this.Custom(enumCodeExists);
        }
    }
}
