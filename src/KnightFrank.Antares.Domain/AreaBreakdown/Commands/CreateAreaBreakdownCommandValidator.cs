namespace KnightFrank.Antares.Domain.AreaBreakdown.Commands
{
    using FluentValidation;
    public class CreateAreaBreakdownCommandValidator : AbstractValidator<CreateAreaBreakdownCommand>
    {
        public CreateAreaBreakdownCommandValidator()
        {
            this.RuleFor(x => x.PropertyId).NotEmpty();
            this.RuleFor(x => x.Areas).NotNull().SetCollectionValidator(new AreaValidator());
        }
    }
}