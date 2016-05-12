namespace KnightFrank.Antares.Domain.Offer.Queries
{
    using FluentValidation;

    public class OfferQueryValidator : AbstractValidator<OfferQuery>
    {
        public OfferQueryValidator()
        {
            this.RuleFor(q => q).NotNull();
            this.RuleFor(q => q.Id).NotNull();
        }
    }
}
