namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    using MediatR;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, Guid>
    {
        private const int NextCallDateDays = 14;
        private readonly IGenericRepository<Activity> activityRepository;

        private readonly IActivityTypeDefinitionValidator activityTypeDefinitionValidator;

        private readonly ICollectionValidator collectionValidator;

        private readonly IGenericRepository<Contact> contactRepository;

        private readonly IEntityValidator entityValidator;

        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;

        private readonly IEnumTypeItemValidator enumTypeItemValidator;

        private readonly IGenericRepository<Property> propertyRepository;

        private readonly IGenericRepository<User> userRepository;

        public CreateActivityCommandHandler(
            IGenericRepository<Activity> activityRepository,
            IGenericRepository<Contact> contactRepository,
            IGenericRepository<User> userRepository,
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            ICollectionValidator collectionValidator,
            IGenericRepository<Property> propertyRepository,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository,
            IActivityTypeDefinitionValidator activityTypeDefinitionValidator)
        {
            this.activityRepository = activityRepository;
            this.contactRepository = contactRepository;
            this.userRepository = userRepository;
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.collectionValidator = collectionValidator;
            this.propertyRepository = propertyRepository;
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.activityTypeDefinitionValidator = activityTypeDefinitionValidator;
        }

        public Guid Handle(CreateActivityCommand message)
        {
            this.entityValidator.EntityExists<Dal.Model.Property.Activities.ActivityType>(message.ActivityTypeId);
            this.enumTypeItemValidator.ItemExists(EnumType.ActivityStatus, message.ActivityStatusId);

            Property property = this.propertyRepository.GetById(message.PropertyId);
            this.entityValidator.EntityExists(property, message.PropertyId);

            this.activityTypeDefinitionValidator.Validate(
                message.ActivityTypeId,
                property.Address.CountryId,
                property.PropertyTypeId);

            var activity = Mapper.Map<Activity>(message);

            List<Contact> vendors = this.contactRepository.FindBy(x => message.ContactIds.Contains(x.Id)).ToList();
            this.collectionValidator.CollectionContainsAll(
                vendors.Select(c => c.Id),
                message.ContactIds,
                ErrorMessage.Missing_Activity_Vendors_Id);

            activity.Contacts = vendors;

            User negotiator =
                this.userRepository.GetWithInclude(x => message.LeadNegotiatorId == x.Id, x => x.Department).SingleOrDefault();
            this.entityValidator.EntityExists(negotiator, message.LeadNegotiatorId);

            activity.ActivityUsers.Add(
                new ActivityUser
                {
                    User = negotiator,
                    UserType = this.GetLeadNegotiatorUserType(),
                    CallDate = DateTime.UtcNow.AddDays(NextCallDateDays).Date
                });

            activity.ActivityDepartments.Add(
                new ActivityDepartment
                    {
                        Department = negotiator.Department,
                        DepartmentType = this.GetManagingDepartmentType()
                    });

            this.activityRepository.Add(activity);
            this.activityRepository.Save();

            return activity.Id;
        }

        private EnumTypeItem GetLeadNegotiatorUserType()
        {
            return this.enumTypeItemRepository.FindBy(i => i.Code == ActivityUserType.LeadNegotiator.ToString()).Single();
        }

        private EnumTypeItem GetManagingDepartmentType()
        {
            return this.enumTypeItemRepository.FindBy(i => i.Code == ActivityDepartmentType.Managing.ToString()).Single();
        }
    }
}
