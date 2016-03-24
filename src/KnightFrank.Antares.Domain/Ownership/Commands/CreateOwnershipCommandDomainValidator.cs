namespace KnightFrank.Antares.Domain.Ownership.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common;

    public class CreateOwnershipCommandDomainValidator : AbstractValidator<CreateOwnershipCommand>, IDomainValidator<CreateOwnershipCommand>
    {
        private readonly IGenericRepository<Ownership> ownershipRepository;

        public CreateOwnershipCommandDomainValidator(IGenericRepository<Ownership> ownershipRepository)
        {
            this.ownershipRepository = ownershipRepository;
            this.Custom(this.DatesDoNotOverlapValidator());
        }

        private Func<CreateOwnershipCommand, ValidationFailure> DatesDoNotOverlapValidator()
        {
            return commmand =>
            {
                List<Range<DateTime>> existingDates = this.ownershipRepository
                    .FindBy(x=> x.PropertyId.Equals(commmand.PropertyId) && x.OwnershipTypeId.Equals(commmand.OwnershipTypeId))
                    .Select(
                        x => new Range<DateTime>(x.PurchaseDate.GetValueOrDefault(DateTime.MinValue), x.SellDate.GetValueOrDefault(DateTime.MaxValue)))
                    .ToList();

                var datesToSave = new Range<DateTime>(commmand.PurchaseDate.GetValueOrDefault(DateTime.MinValue), commmand.SellDate.GetValueOrDefault(DateTime.MaxValue));

                bool overlap = existingDates.Any(existingDate => existingDate.IsOverlapped(datesToSave));

                return overlap ? new ValidationFailure(nameof(commmand.PurchaseDate), "The ownership dates overlap.") : null;
            };
        }
    }
}
