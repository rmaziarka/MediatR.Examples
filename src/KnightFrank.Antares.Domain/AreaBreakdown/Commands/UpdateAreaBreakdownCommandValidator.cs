namespace KnightFrank.Antares.Domain.AreaBreakdown.Commands
{
    using FluentValidation;
    public class UpdateAreaBreakdownCommandValidator : AbstractValidator<UpdateAreaBreakdownCommand>
    {
        public UpdateAreaBreakdownCommandValidator()
        {
            this.RuleFor(x => x.PropertyId).NotEmpty();
            this.RuleFor(x => x.AreaId).NotEmpty();
            this.RuleFor(x => x.Name).Length(0, 128);
            this.RuleFor(x => x.Size).GreaterThan(0);
            this.RuleFor(x => x.Order).GreaterThanOrEqualTo(0);
        }
    }
}