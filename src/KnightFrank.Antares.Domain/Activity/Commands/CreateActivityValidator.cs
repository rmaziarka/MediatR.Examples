namespace KnightFrank.Antares.Domain.Activity.Commands
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

    public class CreateActivityValidator : AbstractValidator<CreateActivityCommand>
    {   
        public CreateActivityValidator(IGenericRepository<Property> propertyRepository, IGenericRepository<EnumTypeItem> enumTypeItemRepository, IReadGenericRepository<Contact> contactRepository)
        {
            Func<CreateActivityCommand, ValidationFailure> propertyExists = cmd =>
            {
                string propertyid = nameof(cmd.PropertyId);
                Property property = propertyRepository.GetById(cmd.PropertyId);

                return property == null ? new ValidationFailure(propertyid, "Property does not exist.") : null;
            };

            Func<CreateActivityCommand, ValidationFailure> activityStatusExists = cmd =>
            {
                string activityStatusId = nameof(cmd.ActivityStatusId);
                EnumTypeItem activityStatus = enumTypeItemRepository.GetById(cmd.ActivityStatusId);

                return activityStatus == null ? new ValidationFailure(activityStatusId, "Activity Status does not exist.") : null;
            };

            Func<CreateActivityCommand, ValidationFailure> isValidvendorsExist = cmd =>
            {
                var isValid = true;
                
                IList<Guid> vendorsId = cmd.Vendors?.Select(v => v.Id).ToList();
                if (vendorsId != null && vendorsId.Any())
                {
                    isValid = contactRepository.Get().All(c => vendorsId.Contains(c.Id));
                }

                return isValid ? null : new ValidationFailure("Vendors", "Vendors are invalid.");
            };

            this.RuleFor(x => x.PropertyId).NotEmpty();
            this.RuleFor(x => x.ActivityStatusId).NotEmpty();

            this.Custom(propertyExists);
            this.Custom(activityStatusExists);
            this.Custom(isValidvendorsExist);
        }
    }
}