namespace KnightFrank.Antares.Domain.AreaBreakdown.Commands
{
    using FluentValidation;
    public class UpdateAreaBreakdownOrderCommandValidator : AbstractValidator<UpdateAreaBreakdownOrderCommand>
    {
        public UpdateAreaBreakdownOrderCommandValidator()
        {
            this.RuleFor(x => x.PropertyId).NotEmpty();
            this.RuleFor(x => x.AreaId).NotEmpty();
            this.RuleFor(x => x.Order).GreaterThanOrEqualTo(0);
        }
    }
}