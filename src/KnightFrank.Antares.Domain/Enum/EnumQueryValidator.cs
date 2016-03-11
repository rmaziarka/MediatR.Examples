namespace KnightFrank.Antares.Domain.Enum
{
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Repository;

    public class EnumQueryValidator : AbstractValidator<EnumQuery>
    {
        public EnumQueryValidator(IReadGenericRepository<EnumType> enumTypeRepository)
        {
            this.Custom(
                query =>
                    {
                        string propertyName = nameof(query.Code);

                        EnumType enumType = enumTypeRepository.Get().SingleOrDefault(x => x.Code == query.Code);

                        return enumType == null ? new ValidationFailure(propertyName, "Enum does not exist.") : null;
                    });
        }
    }
}