namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    using FluentValidation;

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

            this.RuleFor(x => x.ActivityStatusId).NotEmpty();
            this.RuleFor(x => x.ActivityStatusId)
                .SetValidator(new ActivityStatusValidator(enumTypeItemRepository))
                .When(x => !x.ActivityStatusId.Equals(Guid.Empty));

            this.RuleFor(x => x.ActivityTypeId).NotEmpty();
            this.RuleFor(x => x.ActivityTypeId)
                .SetValidator(new ActivityTypeValidator(activityTypeRepository))
                .When(x => !x.ActivityTypeId.Equals(Guid.Empty));

            this.RuleFor(x => x.LeadNegotiatorId).NotEmpty();
            this.RuleFor(x => x.SecondaryNegotiatorIds).NotNull();

            this.RuleFor(x => x).Must(this.ActivityExists).WithMessage("Activity does not exist.")
                .WithName("Id").DependentRules(c => c.RuleFor(x => x).Must(this.ActivityTypeDefinitionExists)
                                                     .WithMessage("Specified activity type is invalid")
                                                     .WithName("ActivityTypeId"));
        }

        private bool ActivityExists(UpdateActivityCommand command)
        {
            Activity activity = this.activityRepository.GetById(command.Id);

            return activity != null;
        }

        private bool ActivityTypeDefinitionExists(UpdateActivityCommand command)
        {
            Activity activity = this.activityRepository.GetById(command.Id);

            bool activityTypeDefinitionExists = this.activityTypeDefinitionRepository.Any(
                x =>
                    x.CountryId == activity.Property.Address.CountryId &&
                    x.PropertyTypeId == activity.Property.PropertyTypeId &&
                    x.ActivityTypeId == command.ActivityTypeId);

            return activityTypeDefinitionExists;
        }
    }
}
