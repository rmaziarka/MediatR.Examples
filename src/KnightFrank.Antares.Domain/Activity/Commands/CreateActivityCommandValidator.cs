namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;
    using System.Linq;

    using FluentValidation;
    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.Validator;

    public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
    {
        public CreateActivityCommandValidator(IGenericRepository<Property> propertyRepository, IGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            Func<CreateActivityCommand, ValidationFailure> propertyExists = cmd =>
            {
                Property property = propertyRepository.GetById(cmd.PropertyId);

                return property == null ? new ValidationFailure(nameof(cmd.PropertyId), "Property does not exist.") : null;
            };

            this.RuleFor(x => x.PropertyId).NotEmpty();
            this.RuleFor(x => x.ActivityStatusId).NotEmpty().SetValidator(new ActivityStatusValidator(enumTypeItemRepository));

            this.Custom(propertyExists);
        }
    }
}