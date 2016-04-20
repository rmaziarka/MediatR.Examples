namespace KnightFrank.Antares.Domain.Characteristic.Queries
{
    using FluentValidation;

    public class CharacteristicGroupsQueryValidator : AbstractValidator<CharacteristicGroupsQuery>
    {
        public CharacteristicGroupsQueryValidator()
        {
            this.RuleFor(x => x.CountryCode).NotEmpty();
            this.RuleFor(x => x.PropertyTypeId).NotEmpty();
        }
    }
}