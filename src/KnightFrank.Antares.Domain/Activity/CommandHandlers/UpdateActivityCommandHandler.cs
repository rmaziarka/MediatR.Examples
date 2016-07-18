namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Portal;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common.Extensions;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    using MediatR;

    using ActivityType = KnightFrank.Antares.Domain.Common.Enums.ActivityType;
    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, Guid>
    {
        private readonly IGenericRepository<Activity> activityRepository;
        private readonly IGenericRepository<Dal.Model.Property.Activities.ActivityType> activityTypeRepository;
        private readonly IActivityTypeDefinitionValidator activityTypeDefinitionValidator;
        private readonly IEntityValidator entityValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IEntityMapper<Activity> activityEntityMapper;
        private readonly IAttributeValidator<Tuple<PropertyType, ActivityType>> attributeValidator;
        private readonly IActivityReferenceMapper<Contact> contactsMapper;
        private readonly IActivityReferenceMapper<ActivityUser> usersMapper;
        private readonly IActivityReferenceMapper<ActivityDepartment> departmentsMapper;
        private readonly IActivityReferenceMapper<Portal> portalsMapper;
        private readonly IActivityReferenceMapper<ActivityAttendee> attendeesMapper;

        public UpdateActivityCommandHandler(
            IGenericRepository<Activity> activityRepository,
            IGenericRepository<Dal.Model.Property.Activities.ActivityType> activityTypeRepository,
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IActivityTypeDefinitionValidator activityTypeDefinitionValidator,
            IEntityMapper<Activity> activityEntityMapper,
            IAttributeValidator<Tuple<PropertyType, ActivityType>> attributeValidator,
            IActivityReferenceMapper<Contact> contactsMapper,
            IActivityReferenceMapper<ActivityUser> usersMapper,
            IActivityReferenceMapper<ActivityDepartment> departmentsMapper,
            IActivityReferenceMapper<Portal> portalsMapper,
            IActivityReferenceMapper<ActivityAttendee> attendeesMapper)
        {
            this.activityRepository = activityRepository;
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.activityTypeDefinitionValidator = activityTypeDefinitionValidator;
            this.activityEntityMapper = activityEntityMapper;
            this.attributeValidator = attributeValidator;
            this.contactsMapper = contactsMapper;
            this.usersMapper = usersMapper;
            this.departmentsMapper = departmentsMapper;
            this.attendeesMapper = attendeesMapper;
            this.portalsMapper = portalsMapper;
            this.activityTypeRepository = activityTypeRepository;
        }

        public Guid Handle(UpdateActivityCommand message)
        {
            Activity activity =
                this.activityRepository.GetWithInclude(
                    x => x.Id == message.Id,
                    x => x.ActivityUsers,
                    x => x.AdvertisingPortals,
                    x => x.Contacts,
                    x => x.ActivityDepartments,
                    x => x.AppraisalMeetingAttendees,
                    x => x.Property.PropertyType,
                    x => x.Property.Address,
                    x => x.ActivityType).SingleOrDefault();

            this.entityValidator.EntityExists(activity, message.Id);

            this.ValidateMessage(message, activity);

            activity.ActivityTypeId = message.ActivityTypeId;
            activity.ActivityType = this.activityTypeRepository.GetById(message.ActivityTypeId);

            activity = this.activityEntityMapper.MapAllowedValues(message, activity, PageType.Update);

            this.contactsMapper.ValidateAndAssign(message, activity);
            this.usersMapper.ValidateAndAssign(message, activity);
            this.departmentsMapper.ValidateAndAssign(message, activity);
            this.attendeesMapper.ValidateAndAssign(message, activity);
            this.portalsMapper.ValidateAndAssign(message, activity);

            this.activityRepository.Save();

            return activity.Id;
        }

        private void ValidateMessage(UpdateActivityCommand message, Activity activity)
        {
            Dal.Model.Property.Activities.ActivityType activityType = this.activityTypeRepository.GetById(message.ActivityTypeId);
            this.entityValidator.EntityExists(activityType, message.ActivityTypeId);
            var activityTypeEnum = EnumExtensions.ParseEnum<ActivityType>(activityType.EnumCode);
            var propertyTypeEnum = EnumExtensions.ParseEnum<PropertyType>(activity.Property.PropertyType.EnumCode);

            this.attributeValidator.Validate(PageType.Update, new Tuple<PropertyType, ActivityType>(propertyTypeEnum, activityTypeEnum), message);

            this.enumTypeItemValidator.ItemExists(EnumType.ActivityStatus, message.ActivityStatusId);

            this.entityValidator.EntityExists<Dal.Model.Property.Activities.ActivityType>(message.ActivityTypeId);

            this.activityTypeDefinitionValidator.Validate(
                message.ActivityTypeId,
                activity.Property.Address.CountryId,
                activity.Property.PropertyTypeId);
        }
    }
}
