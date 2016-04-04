namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;

    public class CreateActivityValidator : AbstractValidator<CreateActivityCommand>
    {   
        public CreateActivityValidator(IGenericRepository<Property> propertyRepository, IGenericRepository<EnumTypeItem> enumTypeItemRepository)
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
            
            this.RuleFor(x => x.PropertyId).NotEmpty();
            this.RuleFor(x => x.ActivityStatusId).NotEmpty();

            this.Custom(propertyExists);
            this.Custom(activityStatusExists);
        }
    }
}