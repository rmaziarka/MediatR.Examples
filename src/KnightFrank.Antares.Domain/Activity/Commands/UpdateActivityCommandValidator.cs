namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Validator;

    public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommand>
    {
        private readonly IGenericRepository<Activity> activityRepository;
        private readonly IGenericRepository<ActivityTypeDefinition> activityTypeDefinitionRepository; 

        public UpdateActivityCommandValidator(IGenericRepository<EnumTypeItem> enumTypeItemRepository, 
            IGenericRepository<Activity> activityRepository, 
            IGenericRepository<ActivityType> activityTypeRepository, 
            IGenericRepository<ActivityTypeDefinition> activityTypeDefinitionRepository)
        {
            this.activityRepository = activityRepository;
            this.activityTypeDefinitionRepository = activityTypeDefinitionRepository;

            this.RuleFor(x => x.MarketAppraisalPrice).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.RecommendedPrice).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.VendorEstimatedPrice).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.ActivityStatusId).SetValidator(new ActivityStatusValidator(enumTypeItemRepository));
            
            this.RuleFor(x => x.ActivityTypeId).NotEmpty();
            this.RuleFor(x => x.ActivityTypeId)
                .SetValidator(new ActivityTypeValidator(activityTypeRepository))
                .When(x => !x.ActivityTypeId.Equals(Guid.Empty));

            this.Custom(this.ActivityExists);
            this.Custom(this.ActivityTypeDefinitionExists);
        }

        private ValidationFailure ActivityExists(UpdateActivityCommand command)
        {
            Activity activity = this.activityRepository.GetById(command.Id);

            return activity == null ? new ValidationFailure(nameof(command.Id), "Activity does not exist.") : null;
        }

        private ValidationFailure ActivityTypeDefinitionExists(UpdateActivityCommand command)
        {
            Activity activity = this.activityRepository.GetById(command.Id);
            var activityTypeDefinitionExists = this.activityTypeDefinitionRepository.Any(
                x =>
                    x.CountryId == activity.Property.Address.CountryId &&
                    x.PropertyTypeId == activity.Property.PropertyTypeId &&
                    x.ActivityTypeId == command.ActivityTypeId);

            return activityTypeDefinitionExists
                ? null
                : new ValidationFailure(nameof(command.ActivityTypeId), "Specified activity type is invalid");
        }
    }
}