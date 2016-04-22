namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Validator;

    public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
    {
        private readonly IGenericRepository<Property> propertyRepository;
        private readonly IGenericRepository<ActivityTypeDefinition> activityTypeDefinitionRepository;

        public CreateActivityCommandValidator(IGenericRepository<Property> propertyRepository,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository,
            IGenericRepository<ActivityType> activityTypeRepository,
            IGenericRepository<ActivityTypeDefinition> activityTypeDefinitionRepository)
        {
            this.propertyRepository = propertyRepository;
            this.activityTypeDefinitionRepository = activityTypeDefinitionRepository;

            this.RuleFor(x => x.PropertyId).NotEmpty();
            this.RuleFor(x => x.ActivityStatusId).NotEmpty()
                .SetValidator(new ActivityStatusValidator(enumTypeItemRepository));

            this.RuleFor(x => x.ActivityTypeId).NotEmpty();
            this.RuleFor(x => x.ActivityTypeId)
                .SetValidator(new ActivityTypeValidator(activityTypeRepository))
                .When(x => !x.ActivityTypeId.Equals(Guid.Empty));

            this.Custom(this.PropertyExists);
            this.Custom(this.ActivityTypeDefinitionExists);
        }

        private ValidationFailure PropertyExists(CreateActivityCommand cmd)
        {
            Property property = this.propertyRepository.GetById(cmd.PropertyId);
            return property == null ? new ValidationFailure(nameof(cmd.PropertyId), "Property does not exist.") : null;
        }

        private ValidationFailure ActivityTypeDefinitionExists(CreateActivityCommand cmd)
        {
            Property property = this.propertyRepository.GetById(cmd.PropertyId);
            var activityTypeDefinitionExists = this.activityTypeDefinitionRepository.Any(
                x =>
                    x.CountryId == property.Address.CountryId &&
                    x.PropertyTypeId == property.PropertyTypeId &&
                    x.ActivityTypeId == cmd.ActivityTypeId);

            return activityTypeDefinitionExists
                ? null
                : new ValidationFailure(nameof(cmd.ActivityTypeId), "Specified activity type is invalid");
        }
    }
}