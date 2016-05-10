namespace KnightFrank.Antares.Domain.Ownership.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Ownership.Commands;

    using MediatR;

    public class CreateOwnershipCommandHandler : IRequestHandler<CreateOwnershipCommand, Guid>
    {
        private readonly IGenericRepository<Ownership> ownershipRepository;
        private readonly IGenericRepository<Contact> contactRepository;
        private readonly IGenericRepository<Property> propertyRepository;
        private readonly IEntityValidator entityValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly ICollectionValidator collectionValidator;

        public CreateOwnershipCommandHandler(IGenericRepository<Ownership> ownershipRepository,
            IGenericRepository<Contact> contactRepository,
            IGenericRepository<Property> propertyRepository,
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            ICollectionValidator collectionValidator)
        {
            this.ownershipRepository = ownershipRepository;
            this.contactRepository = contactRepository;
            this.propertyRepository = propertyRepository;
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.collectionValidator = collectionValidator;
        }

        public Guid Handle(CreateOwnershipCommand message)
        {
            this.enumTypeItemValidator.ItemExists(EnumType.OwnershipType, message.OwnershipTypeId);

            Property property = this.propertyRepository
                                        .GetWithInclude(p => p.Id == message.PropertyId, p => p.Ownerships)
                                        .FirstOrDefault();

            this.entityValidator.EntityExists(property, message.PropertyId);

            // ReSharper disable once PossibleNullReferenceException
            this.DatesDoNotOverlapValidator(message, property.Ownerships);

            var ownership = Mapper.Map<Ownership>(message);

            List<Contact> existingContacts = this.contactRepository.FindBy(x => message.ContactIds.Any(id => id == x.Id)).ToList();
            ownership.Contacts = existingContacts;

            this.ownershipRepository.Add(ownership);
            this.ownershipRepository.Save();

            return ownership.Id;
        }

        private void DatesDoNotOverlapValidator(CreateOwnershipCommand command, ICollection<Ownership> ownerships)
        {
            List<Range<DateTime>> existingDates = ownerships
                .Where(x => x.OwnershipTypeId.Equals(command.OwnershipTypeId))
                .Select(
                    x => new Range<DateTime>(x.PurchaseDate.GetValueOrDefault(DateTime.MinValue), x.SellDate.GetValueOrDefault(DateTime.MaxValue)))
                .ToList();

            var datesToSave = new Range<DateTime>(command.PurchaseDate.GetValueOrDefault(DateTime.MinValue), command.SellDate.GetValueOrDefault(DateTime.MaxValue));

            this.collectionValidator.RangeDoesNotOverlap(existingDates, datesToSave, ErrorMessage.Ownership_Dates_Overlap);
        }
    }
}
