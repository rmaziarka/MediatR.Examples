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

            this.RuleFor(v => v.ExchangeDate.Value.Date)
                .GreaterThanOrEqualTo(v => DateTime.UtcNow.Date)
                .When(v => v.ExchangeDate.HasValue)
                .OverridePropertyName(nameof(CreateOfferCommand.ExchangeDate));

            this.RuleFor(v => v.CompletionDate.Value.Date)
                .GreaterThanOrEqualTo(v => DateTime.UtcNow.Date)
                .When(v => v.CompletionDate.HasValue)
                .OverridePropertyName(nameof(CreateOfferCommand.CompletionDate));
        }
    }
}