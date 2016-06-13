namespace KnightFrank.Antares.Domain.Offer.Commands
{
    using System;

    using FluentValidation;

    public class UpdateOfferCommandValidator : AbstractValidator<UpdateOfferCommand>
    {
        public UpdateOfferCommandValidator()
        {
            this.RuleFor(v => v.Id).NotEmpty();
            this.RuleFor(x => x.StatusId).NotEmpty();
            this.RuleFor(x => x.Price).GreaterThan(0);
            this.RuleFor(x => x.SpecialConditions).Length(0, 4000);
            this.RuleFor(x => x.OfferDate).NotEmpty();
            this.RuleFor(x => x.MortgageLoanToValue)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(200);
            this.RuleFor(x => x.ProgressComment).Length(0, 4000);
        }
    }
}