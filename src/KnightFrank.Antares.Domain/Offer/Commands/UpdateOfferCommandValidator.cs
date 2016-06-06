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

            this.RuleFor(x => x.OfferDate.Date)
                .LessThanOrEqualTo(x => DateTime.UtcNow.Date)
                .OverridePropertyName(nameof(CreateOfferCommand.OfferDate));

            this.RuleFor(x => x.ExchangeDate.Value.Date)
                .GreaterThanOrEqualTo(x => x.OfferDate.Date)
                .When(x => x.ExchangeDate.HasValue)
                .OverridePropertyName(nameof(CreateOfferCommand.ExchangeDate));

            this.RuleFor(x => x.CompletionDate.Value.Date)
                .GreaterThanOrEqualTo(x => x.OfferDate.Date)
                .When(x => x.CompletionDate.HasValue)
                .OverridePropertyName(nameof(CreateOfferCommand.CompletionDate));
        }
    }
}