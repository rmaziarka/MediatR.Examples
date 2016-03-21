namespace KnightFrank.Antares.Domain.Property.Commands
{
    using FluentValidation;

    public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
    {
        public UpdatePropertyCommandValidator()
        {
            this.RuleFor(v => v.Id).NotNull();
        }
    }
}