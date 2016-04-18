namespace KnightFrank.Antares.Domain.Ownership.Commands
{
    using System;

    using FluentValidation;
    using FluentValidation.Results;
    
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;

    public class CreateOwnershipCommandValidator : AbstractValidator<CreateOwnershipCommand>
    {
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;

        public CreateOwnershipCommandValidator(IGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.enumTypeItemRepository = enumTypeItemRepository;

            this.RuleFor(v => v.PurchaseDate.Value.Date)
                .LessThanOrEqualTo(v => v.SellDate.Value.Date)
                .When(v => v.PurchaseDate.HasValue)
                .When(v => v.SellDate.HasValue)
                .OverridePropertyName("PurchaseDate");
            
            this.RuleFor(v => v.PurchaseDate.Value.Date)
                .LessThanOrEqualTo(v => DateTime.UtcNow.Date)
                .When(v => v.PurchaseDate.HasValue)
                .OverridePropertyName("PurchaseDate");

            this.RuleFor(v => v.SellDate.Value.Date)
                .LessThanOrEqualTo(v => DateTime.UtcNow.Date)
                .When(v => v.SellDate.HasValue)
                .OverridePropertyName("SellDate");

            this.RuleFor(v => v.BuyPrice).GreaterThanOrEqualTo(0);
            this.RuleFor(v => v.SellPrice).GreaterThanOrEqualTo(0);

            this.RuleFor(v => v.ContactIds).NotEmpty();

            this.RuleFor(v => v.OwnershipTypeId).NotEmpty();

            this.Custom(this.OwnershipTypeItemExistsValidator);
        }

        private ValidationFailure OwnershipTypeItemExistsValidator(CreateOwnershipCommand commmand)
        {
            var ownershipTypeItem = this.enumTypeItemRepository.GetById(commmand.OwnershipTypeId);
            return ownershipTypeItem == null ? new ValidationFailure(nameof(commmand.OwnershipTypeId), "Ownership type item does not exist.") : null;
        }
    }
}
