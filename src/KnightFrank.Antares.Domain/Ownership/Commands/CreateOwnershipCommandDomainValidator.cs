namespace KnightFrank.Antares.Domain.Ownership.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;

    public class CreateOwnershipCommandDomainValidator : AbstractValidator<CreateOwnershipCommand>, IDomainValidator<CreateOwnershipCommand>
    {
        private readonly IGenericRepository<Ownership> ownershipRepository;
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;
        private readonly IGenericRepository<Property> propertyRepository;
        private readonly IGenericRepository<Contact> contactRepository;

        public CreateOwnershipCommandDomainValidator(
            IGenericRepository<Ownership> ownershipRepository, 
            IGenericRepository<EnumTypeItem> enumTypeItemRepository, 
            IGenericRepository<Contact> contactRepository, 
            IGenericRepository<Property> propertyRepository)
        {
            this.ownershipRepository = ownershipRepository;
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.contactRepository = contactRepository;
            this.propertyRepository = propertyRepository;

            this.Custom(this.DatesDoNotOverlapValidator);
            this.Custom(this.OwnershipTypeValidator);
            this.Custom(this.PropertyExistsValidator);
            this.Custom(this.ContactsExistValidator);
        }

        private ValidationFailure DatesDoNotOverlapValidator(CreateOwnershipCommand commmand)
        {
            List<Range<DateTime>> existingDates = this.ownershipRepository
                .FindBy(x => x.PropertyId.Equals(commmand.PropertyId) && x.OwnershipTypeId.Equals(commmand.OwnershipTypeId))
                .Select(
                    x => new Range<DateTime>(x.PurchaseDate.GetValueOrDefault(DateTime.MinValue), x.SellDate.GetValueOrDefault(DateTime.MaxValue)))
                .ToList();

            var datesToSave = new Range<DateTime>(commmand.PurchaseDate.GetValueOrDefault(DateTime.MinValue), commmand.SellDate.GetValueOrDefault(DateTime.MaxValue));

            bool overlap = existingDates.Any(existingDate => existingDate.IsOverlapped(datesToSave));

            return overlap ? new ValidationFailure(nameof(commmand.PurchaseDate), "The ownership dates overlap.") : null;
        }

        private ValidationFailure OwnershipTypeValidator(CreateOwnershipCommand commmand)
        {
            var ownershipType = this.enumTypeItemRepository.GetById(commmand.OwnershipTypeId);
            return ownershipType.EnumType.Code.Equals("OwnershipType") ? null : new ValidationFailure(nameof(commmand.OwnershipTypeId), "Ownership type does not match.");
        }

        private ValidationFailure PropertyExistsValidator(CreateOwnershipCommand commmand)
        {
            var property = this.propertyRepository.GetById(commmand.PropertyId);
            return property == null ? new ValidationFailure(nameof(commmand.PropertyId), "Property does not exist.") : null;
        }

        private ValidationFailure ContactsExistValidator(CreateOwnershipCommand commmand)
        {
            var contacts = this.contactRepository.FindBy(x => commmand.ContactIds.Any(id => id == x.Id));
            return !contacts.Count().Equals(commmand.ContactIds.Count) ? new ValidationFailure(nameof(commmand.ContactIds), "Contact list is not correct.") : null;
        }
    }
}
