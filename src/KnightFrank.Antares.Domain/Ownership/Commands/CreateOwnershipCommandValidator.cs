namespace KnightFrank.Antares.Domain.Ownership.Commands
{
    using FluentValidation;

    public class CreateOwnershipCommandValidator : AbstractValidator<CreateOwnershipCommand>
    {
        public CreateOwnershipCommandValidator()   
        {
            this.RuleFor(v => v.PurchaseDate.Value)
                .LessThan(v => v.SellDate)
                .When(v => v.PurchaseDate.HasValue)
                .When(v => v.SellDate.HasValue)
                .OverridePropertyName("PurchaseDate");

            this.RuleFor(v => v.BuyPrice).GreaterThan(0);
            this.RuleFor(v => v.SellPrice).GreaterThan(0);

            this.RuleFor(v => v.ContactIds).NotEmpty();

            this.RuleFor(v => v.OwnershipTypeId).NotEmpty();
        }
    }
}
