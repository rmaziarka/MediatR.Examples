namespace KnightFrank.Antares.Domain.Property.Commands
{
    using FluentValidation;
    public class CreateOrUpdatePropertyCharacteristicValidator : AbstractValidator<CreateOrUpdatePropertyCharacteristic>
    {
        public CreateOrUpdatePropertyCharacteristicValidator()
        {
            this.RuleFor(x => x.CharacteristicId).NotEmpty();

            this.RuleFor(x => x.Text).Length(0, 50);
        }
    }
}