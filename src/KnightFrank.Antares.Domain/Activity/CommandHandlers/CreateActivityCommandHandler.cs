namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentValidation.Results;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common;
    using KnightFrank.Antares.Domain.Common.Exceptions;

    using MediatR;

    public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, Guid>
    {
        private readonly IGenericRepository<Activity> activityRepository;
        private readonly IGenericRepository<Contact> contactRepository;
        private readonly IDomainValidator<CreateActivityCommand> domainValidator;

        public CreateActivityCommandHandler(IGenericRepository<Activity> activityRepository, IGenericRepository<Contact> contactRepository, IDomainValidator<CreateActivityCommand> domainValidator)
        {
            this.activityRepository = activityRepository;
            this.contactRepository = contactRepository;
            this.domainValidator = domainValidator;
        }

        public Guid Handle(CreateActivityCommand message)
        {
            ValidationResult validationResult = this.domainValidator.Validate(message);
            if (!validationResult.IsValid)
            {
                throw new DomainValidationException(validationResult.Errors);
            }

            var activity = AutoMapper.Mapper.Map<Activity>(message);

            List<Contact> vendors = this.contactRepository.FindBy(x => message.ContactIds.Contains(x.Id)).ToList();
            activity.Contacts = vendors;

            this.activityRepository.Add(activity);
            this.activityRepository.Save();

            return activity.Id;
        }
    }
}