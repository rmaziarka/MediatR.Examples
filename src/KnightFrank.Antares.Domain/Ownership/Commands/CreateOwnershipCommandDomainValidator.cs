namespace KnightFrank.Antares.Domain.Ownership.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;
    
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Validator;

    public class CreateOwnershipCommandDomainValidator : AbstractValidator<CreateOwnershipCommand>, IDomainValidator<CreateOwnershipCommand>
    {
        private readonly IGenericRepository<Ownership> ownershipRepository;
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;
        private readonly IGenericRepository<Property> propertyRepository;

        public CreateOwnershipCommandDomainValidator(
            IGenericRepository<Ownership> ownershipRepository, 
            IGenericRepository<EnumTypeItem> enumTypeItemRepository, 
            IGenericRepository<Contact> contactRepository, 
            IGenericRepository<Property> propertyRepository)
        {
            this.ownershipRepository = ownershipRepository;
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.propertyRepository = propertyRepository;

            this.Custom(this.DatesDoNotOverlapValidator);
            this.Custom(this.OwnershipTypeValidator);
            this.Custom(this.PropertyExistsValidator);

            this.RuleFor(x => x.ContactIds).SetValidator(new ContactValidator(contactRepository));
        }

        private ValidationFailure DatesDoNotOverlapValidator(CreateOwnershipCommand command)
        {
            List<Range<DateTime>> existingDates = this.ownershipRepository
                .FindBy(x => x.PropertyId.Equals(command.PropertyId) && x.OwnershipTypeId.Equals(command.OwnershipTypeId))
                .Select(
                    x => new Range<DateTime>(x.PurchaseDate.GetValueOrDefault(DateTime.MinValue), x.SellDate.GetValueOrDefault(DateTime.MaxValue)))
                .ToList();

            var datesToSave = new Range<DateTime>(command.PurchaseDate.GetValueOrDefault(DateTime.MinValue), command.SellDate.GetValueOrDefault(DateTime.MaxValue));

            bool overlap = existingDates.Any(existingDate => existingDate.IsOverlapped(datesToSave));

            return overlap ? new ValidationFailure(nameof(command.PurchaseDate), "The ownership dates overlap.") : null;
        }

        private ValidationFailure OwnershipTypeValidator(CreateOwnershipCommand command)
        {
            EnumTypeItem ownershipType = this.enumTypeItemRepository.GetWithInclude(x => x.Id == command.OwnershipTypeId, x => x.EnumType).Single();
            if (ownershipType.EnumType == null)
            {
                return new ValidationFailure(nameof(command.OwnershipTypeId), "Ownership type does not match.");
            }
            return ownershipType.EnumType.Code.Equals("OwnershipType") ? null : new ValidationFailure(nameof(command.OwnershipTypeId), "Ownership type does not match.");
        }

        private ValidationFailure PropertyExistsValidator(CreateOwnershipCommand command)
        {
            var property = this.propertyRepository.GetById(command.PropertyId);
            return property == null ? new ValidationFailure(nameof(command.PropertyId), "Property does not exist.") : null;
        }
    }
}
