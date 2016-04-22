namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Validator;

    public class UpdateActivityCommandValidator : AbstractValidator<UpdateActivityCommand>
    {
        private readonly IGenericRepository<Activity> activityRepository;

        public UpdateActivityCommandValidator(IGenericRepository<EnumTypeItem> enumTypeItemRepository, IGenericRepository<Activity> activityRepository)
        {
            this.activityRepository = activityRepository;

            this.RuleFor(x => x.MarketAppraisalPrice).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.RecommendedPrice).GreaterThanOrEqualTo(0);
            this.RuleFor(x => x.VendorEstimatedPrice).GreaterThanOrEqualTo(0);

            this.RuleFor(x => x.ActivityStatusId).SetValidator(new ActivityStatusValidator(enumTypeItemRepository));

            this.Custom(this.ActivityIdExists);
        }

        private ValidationFailure ActivityIdExists(UpdateActivityCommand command)
        {
            Activity activity = this.activityRepository.GetById(command.Id);

            return activity == null ? new ValidationFailure(nameof(command.Id), "Activity does not exist.") : null;
        }
    }
}