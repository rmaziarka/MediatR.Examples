namespace KnightFrank.Antares.Domain.Common.Validator
{
    using System;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;

    public class ActivityTypeValidator : AbstractValidator<Guid>
    {
        private readonly IGenericRepository<ActivityType> activityTypeRepository;

        public ActivityTypeValidator(IGenericRepository<ActivityType> activityTypeRepository)
        {
            this.activityTypeRepository = activityTypeRepository;
            this.Custom(this.ActivityTypeExists);
        }

        private ValidationFailure ActivityTypeExists(Guid activityTypeId)
        {
            bool activityTypeExists = this.activityTypeRepository.Any(x=>x.Id.Equals(activityTypeId));
            return activityTypeExists ? null : new ValidationFailure("ActivityTypeId", "Activity Type does not exist.");
        }
    }
}