namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Enum;
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

    public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, Guid>
    {
        private readonly IGenericRepository<Activity> activityRepository;

        private readonly IActivityTypeDefinitionValidator activityTypeDefinitionValidator;

        private readonly IGenericRepository<User> userRepository;

        private readonly IGenericRepository<ActivityUser> activityUserRepository;

        private readonly IGenericRepository<ActivityDepartment> activityDepartmentRepository;

        private readonly ICollectionValidator collectionValidator;

        private readonly IEntityValidator entityValidator;

        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;

        private readonly IEnumTypeItemValidator enumTypeItemValidator;

        private readonly IControlsConfiguration<PropertyType, ActivityType> activityConfiguration;

        private readonly IEntityMapper entityMapper;
        private readonly IAttributeValidator<PropertyType, ActivityType> attributeValidator;

        public UpdateActivityCommandHandler(
            IGenericRepository<Activity> activityRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<ActivityUser> activityUserRepository,
            IGenericRepository<ActivityDepartment> activityDepartmentRepository,
            IEntityValidator entityValidator,
            ICollectionValidator collectionValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IActivityTypeDefinitionValidator activityTypeDefinitionValidator,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository, 
            IControlsConfiguration<PropertyType, ActivityType> activityConfiguration, 
            IEntityMapper entityMapper,
            IAttributeValidator<PropertyType, ActivityType> attributeValidator)

        {
            this.activityRepository = activityRepository;
            this.userRepository = userRepository;
            this.activityUserRepository = activityUserRepository;
            this.activityDepartmentRepository = activityDepartmentRepository;
            this.entityValidator = entityValidator;
            this.collectionValidator = collectionValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.activityTypeDefinitionValidator = activityTypeDefinitionValidator;
            this.enumTypeItemRepository = enumTypeItemRepository;
            this.activityConfiguration = activityConfiguration;
            this.entityMapper = entityMapper;
            this.attributeValidator = attributeValidator;
        }

        public Guid Handle(UpdateActivityCommand message)
        {
            Activity activity =
                this.activityRepository.GetWithInclude(
                    x => x.Id == message.Id,
                    x => x.ActivityUsers,
                    x => x.ActivityDepartments,
                    x => x.Property.PropertyType,
                    x => x.ActivityType)
                    .SingleOrDefault();

            this.entityValidator.EntityExists(activity, message.Id);

            // ReSharper disable once PossibleNullReferenceException
            var activityType = EnumExtensions.ParseEnum<ActivityType>(activity.ActivityType.EnumCode);
            var propertyType = EnumExtensions.ParseEnum<PropertyType>(activity.Property.PropertyType.EnumCode);
            
            this.attributeValidator.Validate(PageType.Update, propertyType, activityType, message);

            this.enumTypeItemValidator.ItemExists(EnumType.ActivityStatus, message.ActivityStatusId);

            this.ValidateActivityTypeFromCommand(message, activity);

            List<ActivityUser> commandNegotiators = this.ValidateAndRetrieveNegotiatorsFromCommand(message);

            activity = this.entityMapper.MapAllowedValues(message, activity, this.activityConfiguration, PageType.Update, propertyType, activityType);

            this.ValidateActivityNegotiators(commandNegotiators, activity);
            this.UpdateActivityNegotiators(commandNegotiators, activity);

            this.ValidateActivityDepartments(message.Departments, activity);
            this.UpdateActivityDepartments(message.Departments, activity);

            this.activityRepository.Save();

            // ReSharper disable once PossibleNullReferenceException
            return activity.Id;
        }

        private void ValidateActivityTypeFromCommand(UpdateActivityCommand message, Activity activity)
        {
            this.entityValidator.EntityExists<Dal.Model.Property.Activities.ActivityType>(message.ActivityTypeId);

            // ReSharper disable once PossibleNullReferenceException
            this.activityTypeDefinitionValidator.Validate(
                message.ActivityTypeId,
                activity.Property.Address.CountryId,
                activity.Property.PropertyTypeId);
        }

        private List<ActivityUser> ValidateAndRetrieveNegotiatorsFromCommand(UpdateActivityCommand message)
        {
            // Lead negotiator
            this.entityValidator.EntityExists<User>(message.LeadNegotiator.UserId);

            User leadNegotiator = this.GetUser(message.LeadNegotiator.UserId);
            var commandNegotiators = new List<ActivityUser>
                                         {
                                             new ActivityUser
                                                 {
                                                     UserId = leadNegotiator.Id,
                                                     User = leadNegotiator,
                                                     UserType = this.GetLeadNegotiatorUserType(),
                                                     CallDate = message.LeadNegotiator.CallDate
                                                 }
                                         };

            //Secondary negotiators
            this.entityValidator.EntitiesExist<User>(message.SecondaryNegotiators.Select(n => n.UserId).ToList());

            commandNegotiators.AddRange(
                message.SecondaryNegotiators.Select(
                    n =>
                        {
                            User user = this.GetUser(n.UserId);
                            return new ActivityUser
                                       {
                                           UserId = user.Id,
                                           User = user,
                                           UserType = this.GetSecondaryNegotiatorUserType(),
                                           CallDate = n.CallDate
                                       };
                        }));

            // All negotirators
            this.collectionValidator.CollectionIsUnique(
                commandNegotiators.Select(n => n.UserId).ToList(),
                ErrorMessage.Activity_Negotiators_Not_Unique);

            return commandNegotiators;
        }

        private void ValidateActivityNegotiators(List<ActivityUser> commandNegotiators, Activity activity)
        {
            List<ActivityUser> existingNegotiators = activity.ActivityUsers.ToList();

            commandNegotiators.Where(n => IsNewlyAddedToExistingList(n, existingNegotiators))
                              .ToList()
                              .ForEach(n => ValidateNegotiatorCallDate(n.CallDate, null));

            commandNegotiators.Where(n => IsUpdated(n, existingNegotiators))
                              .Select(n => new { newNegotiator = n, oldNegotiator = GetOldActivityUser(n, existingNegotiators) })
                              .ToList()
                              .ForEach(pair => ValidateNegotiatorCallDate(pair.newNegotiator.CallDate, pair.oldNegotiator.CallDate));
        }
        private static void ValidateNegotiatorCallDate(DateTime? newNegotiatorCallDate, DateTime? oldNegotiatorCallDate)
        {
            if (!newNegotiatorCallDate.HasValue)
            {
                return;
            }

            if (oldNegotiatorCallDate.HasValue && (newNegotiatorCallDate.Value.Date == oldNegotiatorCallDate.Value.Date))
            {
                return;
            }

            if (newNegotiatorCallDate.Value.Date < DateTime.UtcNow.Date)
            {
                throw new BusinessValidationException(ErrorMessage.Activity_Negotiator_CallDate_InPast);
            }
        }

        private void UpdateActivityNegotiators(List<ActivityUser> commandNegotiators, Activity activity)
        {
            List<ActivityUser> existingNegotiators = activity.ActivityUsers.ToList();

            existingNegotiators.Where(n => IsRemovedFromExistingList(n, commandNegotiators))
                               .ToList()
                               .ForEach(n => this.activityUserRepository.Delete(n));

            commandNegotiators.Where(n => IsNewlyAddedToExistingList(n, existingNegotiators))
                              .ToList()
                              .ForEach(n => activity.ActivityUsers.Add(n));

            commandNegotiators.Where(n => IsUpdated(n, existingNegotiators))
                              .Select(n => new { newNagotiator = n, oldNegotiator = GetOldActivityUser(n, existingNegotiators) })
                              .ToList()
                              .ForEach(pair => UpdateNegotiator(pair.newNagotiator, pair.oldNegotiator));
        }

        private static bool IsRemovedFromExistingList(ActivityUser existingActivityUser, IEnumerable<ActivityUser> activityUsers)
        {
            return !activityUsers.Select(x => x.UserId).Contains(existingActivityUser.UserId);
        }

        private static bool IsNewlyAddedToExistingList(ActivityUser activityUser, IEnumerable<ActivityUser> existingActivityUsers)
        {
            return !existingActivityUsers.Select(x => x.UserId).Contains(activityUser.UserId);
        }

        private static bool IsUpdated(ActivityUser activityUser, IEnumerable<ActivityUser> existingActivityUsers)
        {
            return !IsNewlyAddedToExistingList(activityUser, existingActivityUsers);
        }

        private static ActivityUser GetOldActivityUser(ActivityUser activityUser, IEnumerable<ActivityUser> existingActivityUsers)
        {
            return existingActivityUsers.SingleOrDefault(x => x.UserId == activityUser.UserId);
        }

        private static void UpdateNegotiator(ActivityUser newNegotiator, ActivityUser oldNegotiator)
        {
            oldNegotiator.UserType = newNegotiator.UserType;
            oldNegotiator.CallDate = newNegotiator.CallDate?.Date;
        }

        private void ValidateActivityDepartments(
            IList<UpdateActivityDepartment> updateActivityDepartments,
            Activity activity)
        {
            this.collectionValidator.CollectionIsUnique(
                updateActivityDepartments.Select(x => x.DepartmentId).ToList(),
                ErrorMessage.Activity_Departments_Not_Unique);

            updateActivityDepartments.ToList()
                                     .ForEach(
                                         x =>
                                         this.enumTypeItemValidator.ItemExists(EnumType.ActivityDepartmentType, x.DepartmentTypeId));

            this.entityValidator.EntitiesExist<Department>(updateActivityDepartments.Select(x => x.DepartmentId).ToList());

            Guid managingDepartmentTypeId = this.GetManagingDepartmentType().Id;
            if (updateActivityDepartments.Count(x => x.DepartmentTypeId == managingDepartmentTypeId) != 1)
            {
                throw new BusinessValidationException(ErrorMessage.Activity_Should_Have_Only_One_Managing_Department);
            }

            List<Guid> activityUserDepartmentIds = activity.ActivityUsers.Select(x => x.User.DepartmentId).ToList();
            List<ActivityDepartment> existingDepartments = activity.ActivityDepartments.ToList();
            // find newly addded departments, which have no related users
            bool hasInvalidDepartments =
                updateActivityDepartments.Any(
                    d => IsNewlyAddedToExistingList(d, existingDepartments) && !activityUserDepartmentIds.Contains(d.DepartmentId));
            if (hasInvalidDepartments)
            {
                throw new BusinessValidationException(ErrorMessage.ActivityDepartment_Invalid_Value);
            }
        }

        private void UpdateActivityDepartments(
            IList<UpdateActivityDepartment> updateActivityDepartments,
            Activity activity)
        {
            List<ActivityDepartment> existingDepartments = activity.ActivityDepartments.ToList();

            existingDepartments.Where(d => IsRemovedFromExistingList(d, updateActivityDepartments))
                               .ToList()
                               .ForEach(d => this.activityDepartmentRepository.Delete(d));

            updateActivityDepartments.Where(d => IsNewlyAddedToExistingList(d, existingDepartments))
                                     .ToList()
                                     .ForEach(d => activity.ActivityDepartments.Add(CreateActivityDepartament(d)));

            updateActivityDepartments.Where(d => IsUpdated(d, existingDepartments))
                              .Select(d => new { updateActivityDepartment = d, activityDepartment = GetOldActivityDepartment(d, existingDepartments) })
                              .ToList()
                              .ForEach(pair => UpdateActivityDepartament(pair.updateActivityDepartment, pair.activityDepartment));
        }

        private static bool IsRemovedFromExistingList(ActivityDepartment existingActivityDepartment, IList<UpdateActivityDepartment> updateActivityDepartments)
        {
            return !updateActivityDepartments.Select(x => x.DepartmentId).Contains(existingActivityDepartment.DepartmentId);
        }

        private static bool IsNewlyAddedToExistingList(UpdateActivityDepartment updateActivityDepartment, IEnumerable<ActivityDepartment> existingActivityDepartments)
        {
            return !existingActivityDepartments.Select(x => x.DepartmentId).Contains(updateActivityDepartment.DepartmentId);
        }

        private static bool IsUpdated(UpdateActivityDepartment updateActivityDepartment, IEnumerable<ActivityDepartment> existingActivityDepartments)
        {
            return !IsNewlyAddedToExistingList(updateActivityDepartment, existingActivityDepartments);
        }

        private static ActivityDepartment GetOldActivityDepartment(UpdateActivityDepartment updateActivityDepartment, IEnumerable<ActivityDepartment> existingActivityDepartments)
        {
            return existingActivityDepartments.SingleOrDefault(x => x.DepartmentId == updateActivityDepartment.DepartmentId);
        }

        private static ActivityDepartment CreateActivityDepartament(UpdateActivityDepartment updateActivityDepartment)
        {
            return new ActivityDepartment
                       {
                           DepartmentId = updateActivityDepartment.DepartmentId,
                           DepartmentTypeId = updateActivityDepartment.DepartmentTypeId
                       };
        }

        private static void UpdateActivityDepartament(UpdateActivityDepartment updateActivityDepartment, ActivityDepartment oldDepartament)
        {
            oldDepartament.DepartmentTypeId = updateActivityDepartment.DepartmentTypeId;
        }

        private User GetUser(Guid userId)
        {
            return this.userRepository.GetWithInclude(x => x.Id == userId, x => x.Department).Single();
        }

        private EnumTypeItem GetLeadNegotiatorUserType()
        {
            return this.enumTypeItemRepository.FindBy(i => i.Code == ActivityUserType.LeadNegotiator.ToString()).Single();
        }

        private EnumTypeItem GetSecondaryNegotiatorUserType()
        {
            return this.enumTypeItemRepository.FindBy(i => i.Code == ActivityUserType.SecondaryNegotiator.ToString()).Single();
        }

        private EnumTypeItem GetManagingDepartmentType()
        {
            return this.enumTypeItemRepository.FindBy(i => i.Code == ActivityDepartmentType.Managing.ToString()).Single();
        }
    }
}
