namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;

    using ActivityType = KnightFrank.Antares.Domain.Common.Enums.ActivityType;
    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;
    using PropertyType = KnightFrank.Antares.Domain.Common.Enums.PropertyType;

    public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, Guid>
    {
        private readonly IGenericRepository<Activity> activityRepository;
        private readonly IGenericRepository<Dal.Model.Property.Activities.ActivityType> activityTypeRepository;
        private readonly IActivityTypeDefinitionValidator activityTypeDefinitionValidator;
        private readonly IAttributeValidator<Tuple<PropertyType, ActivityType>> attributeValidator;
        private readonly IEntityValidator entityValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IGenericRepository<Property> propertyRepository;
        private readonly IEntityMapper<Activity> activityEntityMapper;
        private readonly IActivityReferenceMapper<Contact> contactsMapper;
        private readonly IActivityReferenceMapper<ActivityUser> usersMapper;
        private readonly IActivityReferenceMapper<ActivityDepartment> departmentsMapper;
        private readonly IActivityReferenceMapper<ActivityAttendee> attendeesMapper;

        public CreateActivityCommandHandler(
            IGenericRepository<Activity> activityRepository,
            IGenericRepository<Dal.Model.Property.Activities.ActivityType> activityTypeRepository,
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IGenericRepository<Property> propertyRepository,
            IActivityTypeDefinitionValidator activityTypeDefinitionValidator,
            IAttributeValidator<Tuple<PropertyType, ActivityType>> attributeValidator,
            IEntityMapper<Activity> activityEntityMapper,
            IActivityReferenceMapper<Contact> contactsMapper,
            IActivityReferenceMapper<ActivityUser> usersMapper,
            IActivityReferenceMapper<ActivityDepartment> departmentsMapper,
            IActivityReferenceMapper<ActivityAttendee> attendeesMapper
            )
        {
            this.activityRepository = activityRepository;
            this.activityTypeRepository = activityTypeRepository;
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.propertyRepository = propertyRepository;
            this.activityTypeDefinitionValidator = activityTypeDefinitionValidator;
            this.attributeValidator = attributeValidator;
            this.activityEntityMapper = activityEntityMapper;
            this.contactsMapper = contactsMapper;
            this.usersMapper = usersMapper;
            this.departmentsMapper = departmentsMapper;
            this.attendeesMapper = attendeesMapper;
        }

        public Guid Handle(CreateActivityCommand message)
        {
            Property property = this.propertyRepository.GetWithInclude(
                x => x.Id == message.PropertyId,
                x => x.Address,
                x => x.PropertyType).SingleOrDefault();

            this.entityValidator.EntityExists(property, message.PropertyId);

            this.ValidateMessage(message, property);

            var activity = new Activity
            {
                PropertyId = message.PropertyId,
                ActivityTypeId = message.ActivityTypeId
            };

            activity = this.activityEntityMapper.MapAllowedValues(message, activity, PageType.Create);

            this.contactsMapper.ValidateAndAssign(message, activity);
            this.usersMapper.ValidateAndAssign(message, activity);
            this.departmentsMapper.ValidateAndAssign(message, activity);
            this.attendeesMapper.ValidateAndAssign(message, activity);

            this.activityRepository.Add(activity);
            this.activityRepository.Save();

            return activity.Id;
        }

        private void ValidateMessage(CreateActivityCommand message, Property property)
        {
            Dal.Model.Property.Activities.ActivityType activityType = this.activityTypeRepository.GetById(message.ActivityTypeId);
            this.entityValidator.EntityExists(activityType, message.ActivityTypeId);
            var activityTypeEnum = EnumExtensions.ParseEnum<ActivityType>(activityType.EnumCode);
            var propertyTypeEnum = EnumExtensions.ParseEnum<PropertyType>(property.PropertyType.EnumCode);

            this.attributeValidator.Validate(PageType.Create, Tuple.Create(propertyTypeEnum, activityTypeEnum), message);

            this.enumTypeItemValidator.ItemExists(EnumType.ActivityStatus, message.ActivityStatusId);

            this.activityTypeDefinitionValidator.Validate(
                message.ActivityTypeId,
                property.Address.CountryId,
                property.PropertyTypeId);
        }
    }
}
