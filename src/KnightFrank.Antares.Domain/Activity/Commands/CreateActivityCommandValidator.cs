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
        public CreateActivityCommandValidator()
        {
           
            this.RuleFor(x => x.PropertyId).NotEmpty();

            this.RuleFor(x => x.ActivityStatusId).NotEmpty();
           
            this.RuleFor(x => x.ActivityTypeId).NotEmpty();
           
            this.RuleFor(x => x.LeadNegotiatorId).NotEmpty();
        }
    }
}
