namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.EntityConfigurations;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    using MediatR;

    using ActivityType = KnightFrank.Antares.Domain.Common.Enums.ActivityType;
    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;
    using PropertyType = KnightFrank.Antares.Domain.Common.Enums.PropertyType;

    public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, Guid>
    {
        private const int NextCallDateDays = 14;
        private readonly IGenericRepository<Activity> activityRepository;
        private readonly IGenericRepository<Dal.Model.Property.Activities.ActivityType> activityTypeRepository;

        private readonly IActivityTypeDefinitionValidator activityTypeDefinitionValidator;
        private readonly IAttributeValidator<PropertyType, ActivityType> attributeValidator;

        private readonly ICollectionValidator collectionValidator;

        private readonly IGenericRepository<Contact> contactRepository;

        private readonly IEntityValidator entityValidator;

        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;

        private readonly IEnumTypeItemValidator enumTypeItemValidator;

        private readonly IGenericRepository<Property> propertyRepository;

        private readonly IGenericRepository<User> userRepository;

        private readonly IControlsConfiguration<PropertyType, ActivityType> activityConfiguration;

        private readonly IEntityMapper entityMapper;

        public CreateActivityCommandHandler(
            IGenericRepository<Activity> activityRepository,
            IGenericRepository<Dal.Model.Property.Activities.ActivityType> activityTypeRepository,
            IGenericRepository<Contact> contactRepository,
            IGenericRepository<User> userRepository,
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            ICollectionValidator collectionValidator,
            IGenericRepository<Property> propertyRepository,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository,
            IActivityTypeDefinitionValidator activityTypeDefinitionValidator,
            IAttributeValidator<PropertyType, ActivityType> attributeValidator, 
            IControlsConfiguration<PropertyType, ActivityType> activityConfiguration, 
            IEntityMapper entityMapper)
        {
            this.activityRepository = activityRepository;
            this.activityTypeRepository = activityTypeRepository;
            this.contactRepository = contactRepository;
            this.userRepository = userRepository;
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.collectionValidator = collectionValidator;
            this.propertyRepository = propertyRepository;
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.activityTypeDefinitionValidator = activityTypeDefinitionValidator;
            this.attributeValidator = attributeValidator;
            this.activityConfiguration = activityConfiguration;
            this.entityMapper = entityMapper;
        }

        public Guid Handle(CreateActivityCommand message)
        {
            Property property = this.propertyRepository.GetById(message.PropertyId);
            this.entityValidator.EntityExists(property, message.PropertyId);

            this.enumTypeItemValidator.ItemExists(EnumType.ActivityStatus, message.ActivityStatusId);

            this.activityTypeDefinitionValidator.Validate(
                message.ActivityTypeId,
                property.Address.CountryId,
                property.PropertyTypeId);

            this.ValidateMessageAttributes(property.PropertyType.EnumCode, message);

            // ReSharper disable once PossibleNullReferenceException
            var propertyType = (PropertyType)Enum.Parse(typeof(PropertyType), property.PropertyType.EnumCode);
            Dal.Model.Property.Activities.ActivityType activityType = this.activityTypeRepository.GetById(message.ActivityTypeId);
            var activityTypeEnum = EnumExtensions.ParseEnum<ActivityType>(activityType.EnumCode);
            var activity = new Activity
            {
                PropertyId = message.PropertyId,
                ActivityTypeId = message.ActivityTypeId
            };

            activity = this.entityMapper.MapAllowedValues(message, activity, this.activityConfiguration, PageType.Create, propertyType, activityTypeEnum);

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
                    // ReSharper disable once PossibleNullReferenceException
                    Department = negotiator.Department,
                    DepartmentType = this.GetManagingDepartmentType()
                });

            this.activityRepository.Add(activity);
            this.activityRepository.Save();

            return activity.Id;
        }

        private void ValidateMessageAttributes(string propertyTypeCode, CreateActivityCommand message)
        {
            var propertyType = EnumExtensions.ParseEnum<PropertyType>(propertyTypeCode);

            Dal.Model.Property.Activities.ActivityType activityType = this.activityTypeRepository.GetById(message.ActivityTypeId);
            this.entityValidator.EntityExists(activityType, message.ActivityTypeId);

            var activityTypeEnum = EnumExtensions.ParseEnum<ActivityType>(activityType.EnumCode);

            this.attributeValidator.Validate(PageType.Create, propertyType, activityTypeEnum, message);
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
