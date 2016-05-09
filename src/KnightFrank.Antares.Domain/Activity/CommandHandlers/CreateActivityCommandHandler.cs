namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;

    public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, Guid>
    {
        private readonly IGenericRepository<Activity> activityRepository;
        private readonly IGenericRepository<Contact> contactRepository;

        public CreateActivityCommandHandler(IGenericRepository<Activity> activityRepository, IGenericRepository<Contact> contactRepository)
        {
            this.activityRepository = activityRepository;
            this.contactRepository = contactRepository;
        }

        public Guid Handle(CreateActivityCommand message)
        {
            var activity = AutoMapper.Mapper.Map<Activity>(message);

            List<Contact> vendors = this.contactRepository.FindBy(x => message.ContactIds.Contains(x.Id)).ToList();
            if (!message.ContactIds.All(x => vendors.Select(c => c.Id).Contains(x)))
            {
                throw new BusinessValidationException(ErrorMessage.Missing_Activity_Vendors_Id);
            }

            activity.Contacts = vendors;

            this.activityRepository.Add(activity);
            this.activityRepository.Save();

            return activity.Id;
        }
    }
}