namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Common;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    using MediatR;

    public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, Guid>
    {
        private readonly IGenericRepository<Activity> activityRepository;
        private readonly IGenericRepository<Contact> contactRepository;
        private readonly IGenericRepository<User> userRepository;
        private readonly IEntityValidator entityValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly ICollectionValidator collectionValidator;
        private readonly IGenericRepository<Property> propertyRepository;
        private readonly IActivityTypeDefinitionValidator activityTypeDefinitionValidator;

        public CreateActivityCommandHandler(
            IGenericRepository<Activity> activityRepository,
            IGenericRepository<Contact> contactRepository,
            IGenericRepository<User> userRepository,
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            ICollectionValidator collectionValidator,
            IGenericRepository<Property> propertyRepository,
            IActivityTypeDefinitionValidator activityTypeDefinitionValidator)
        {
            this.activityRepository = activityRepository;
            this.contactRepository = contactRepository;
            this.userRepository = userRepository;
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.collectionValidator = collectionValidator;
            this.propertyRepository = propertyRepository;
            this.activityTypeDefinitionValidator = activityTypeDefinitionValidator;
        }

        public Guid Handle(CreateActivityCommand message)
        {

            Property property = this.propertyRepository.GetById(message.PropertyId);
            this.entityValidator.EntityExists(property, message.PropertyId);

            this.activityTypeDefinitionValidator.Validate(message.ActivityTypeId, property.Address.CountryId, property.PropertyTypeId);

            this.entityValidator.EntityExists<ActivityType>(message.ActivityTypeId);
            this.enumTypeItemValidator.ItemExists(EnumType.ActivityStatus, message.ActivityStatusId);

            var activity = AutoMapper.Mapper.Map<Activity>(message);

            List<Contact> vendors = this.contactRepository.FindBy(x => message.ContactIds.Contains(x.Id)).ToList();
            this.collectionValidator.CollectionContainsAll(vendors.Select(c => c.Id), message.ContactIds, ErrorMessage.Missing_Activity_Vendors_Id);

            activity.Contacts = vendors;

            User negotiator = this.userRepository.FindBy(x => message.LeadNegotiatorId == x.Id).SingleOrDefault();
            this.entityValidator.EntityExists(negotiator, message.LeadNegotiatorId);

            activity.ActivityUsers.Add(new ActivityUser
            {
                Activity = activity,
                User = negotiator,
                UserType = UserTypeEnum.LeadNegotiator
            });

            this.activityRepository.Add(activity);
            this.activityRepository.Save();

            return activity.Id;
        }
    }
}