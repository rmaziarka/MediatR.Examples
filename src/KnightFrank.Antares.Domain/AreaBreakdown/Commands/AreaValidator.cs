namespace KnightFrank.Antares.Domain.AreaBreakdown.Commands
{
    using FluentValidation;

    public class AreaValidator : AbstractValidator<Area>
    {
        public AreaValidator()
        {
            this.RuleFor(x => x.Name).Length(0, 128);
            this.RuleFor(x => x.Size).GreaterThan(0);
        }
    }
}