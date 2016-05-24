namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using FluentValidation;

    public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommand>
    {
        public UpdateActivityCommandValidator(
)
        {
            this.RuleFor(x => x.MarketAppraisalPrice).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.RecommendedPrice).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.VendorEstimatedPrice).GreaterThanOrEqualTo(0);

            this.RuleFor(x => x.ActivityStatusId).NotEmpty();

            this.RuleFor(x => x.ActivityTypeId).NotEmpty();
            this.RuleFor(x => x.LeadNegotiatorId).NotEmpty();
            this.RuleFor(x => x.SecondaryNegotiatorIds).NotNull();
        }
    }
}
