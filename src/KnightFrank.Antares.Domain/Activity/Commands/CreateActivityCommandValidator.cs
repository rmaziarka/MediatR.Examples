namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;

    public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
    {   
        public CreateActivityCommandValidator(IGenericRepository<Property> propertyRepository, IReadGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            Func<CreateActivityCommand, ValidationFailure> propertyExists = cmd =>
            {   
                Property property = propertyRepository.GetById(cmd.PropertyId);

                return property == null ? new ValidationFailure(nameof(cmd.PropertyId), "Property does not exist.") : null;
            };

            Func<CreateActivityCommand, ValidationFailure> activityStatusExists = cmd =>
            {
                bool isActivityStatus =
                    enumTypeItemRepository.Get()
                                          .Any(et => et.Id == cmd.ActivityStatusId && et.EnumType.Code == "ActivityStatus");

                return isActivityStatus ? null : new ValidationFailure(nameof(cmd.ActivityStatusId), "Activity Status does not exist.");
            };

            this.RuleFor(x => x.PropertyId).NotEmpty();
            this.RuleFor(x => x.ActivityStatusId).NotEmpty();

            this.Custom(propertyExists);
            this.Custom(activityStatusExists);
        }
    }
}