namespace KnightFrank.Antares.Domain.Common.Validator
{
    using System;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;

    public class ActivityStatusValidator : AbstractValidator<Guid>
    {
        private readonly IReadGenericRepository<EnumTypeItem> enumTypeItemRepository;

        public ActivityStatusValidator(IReadGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.Custom(this.ActivityStatusExists);
        }

        private ValidationFailure ActivityStatusExists(Guid activityStatusId)
        {
            bool activityStatusExists =
                this.enumTypeItemRepository.Get().Any(et => et.Id == activityStatusId && et.EnumType.Code == "ActivityStatus");

            return activityStatusExists ? null : new ValidationFailure(nameof(activityStatusId), "Activity Status does not exist.");
        }
    }
}