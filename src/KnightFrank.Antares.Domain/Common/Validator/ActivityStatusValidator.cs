namespace KnightFrank.Antares.Domain.Common.Validator
{
    using System;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Specifications;
    using KnightFrank.Antares.Domain.Enum.Specifications;

    public class ActivityStatusValidator : AbstractValidator<Guid>
    {
        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;

        public ActivityStatusValidator(IGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.Custom(this.ActivityStatusExists);
        }

        private ValidationFailure ActivityStatusExists(Guid activityStatusId)
        {
            bool activityStatusExists =
                this.enumTypeItemRepository.Any((new HasId<EnumTypeItem>(activityStatusId) & new IsActivityStatus()).SatisfiedBy());

            return activityStatusExists ? null : new ValidationFailure(nameof(activityStatusId), "Activity Status does not exist.");
        }
    }
}