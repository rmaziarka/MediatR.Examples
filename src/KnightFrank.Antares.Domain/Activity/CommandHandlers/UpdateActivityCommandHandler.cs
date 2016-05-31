namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    using MediatR;

    using EnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, Guid>
    {
        private readonly IGenericRepository<Activity> activityRepository;

        private readonly IActivityTypeDefinitionValidator activityTypeDefinitionValidator;

        private readonly IGenericRepository<ActivityUser> activityUserRepository;

        private readonly ICollectionValidator collectionValidator;

        private readonly IEntityValidator entityValidator;

        private readonly IGenericRepository<EnumTypeItem> enumTypeItemRepository;

        private readonly IEnumTypeItemValidator enumTypeItemValidator;

        public UpdateActivityCommandHandler(
            IGenericRepository<Activity> activityRepository,
            IGenericRepository<ActivityUser> activityUserRepository,
            IGenericRepository<ActivityTypeDefinition> activityTypeDefinitionRepository,
            IEntityValidator entityValidator,
            ICollectionValidator collectionValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IActivityTypeDefinitionValidator activityTypeDefinitionValidator,
            IGenericRepository<EnumTypeItem> enumTypeItemRepository)
        {
            this.activityRepository = activityRepository;
            this.activityUserRepository = activityUserRepository;
            this.entityValidator = entityValidator;
            this.collectionValidator = collectionValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.activityTypeDefinitionValidator = activityTypeDefinitionValidator;
            this.enumTypeItemRepository = enumTypeItemRepository;
        }

        public Guid Handle(UpdateActivityCommand message)
        {
            Activity activity =
                this.activityRepository.GetWithInclude(x => x.Id == message.Id, x => x.ActivityUsers).SingleOrDefault();

            this.entityValidator.EntityExists(activity, message.Id);
            this.entityValidator.EntityExists<ActivityType>(message.ActivityTypeId);
            this.enumTypeItemValidator.ItemExists(EnumType.ActivityStatus, message.ActivityStatusId);

            // ReSharper disable once PossibleNullReferenceException
            this.activityTypeDefinitionValidator.Validate(
                message.ActivityTypeId,
                activity.Property.Address.CountryId,
                activity.Property.PropertyTypeId);

            this.entityValidator.EntityExists<User>(message.LeadNegotiator.Id);

            var commandNegotiators = new List<ActivityUser>
                                         {
                                             new ActivityUser
                                                 {
                                                     UserId = message.LeadNegotiator.Id,
                                                     UserType = this.GetLeadNegotiatorUserType(),
                                                     CallDate =  message.LeadNegotiator.CallDate
                                                 }
                                         };

            this.entityValidator.EntitiesExist<User>(message.SecondaryNegotiators.Select(n => n.Id).ToList());

            commandNegotiators.AddRange(
                message.SecondaryNegotiators.Select(
                    n => new ActivityUser { UserId = n.Id, UserType = this.GetSecondaryNegotiatorUserType(), CallDate = n.CallDate }));

            this.collectionValidator.CollectionIsUnique(
                commandNegotiators.Select(n => n.UserId).ToList(),
                ErrorMessage.Activity_Negotiators_Not_Unique);

            Mapper.Map(message, activity);

            // ReSharper disable once PossibleNullReferenceException
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

            this.activityRepository.Save();

            return activity.Id;
        }

        private static void UpdateNegotiator(ActivityUser newNegotiator, ActivityUser oldNegotiator)
        {
            oldNegotiator.UserType = newNegotiator.UserType;
            oldNegotiator.CallDate = newNegotiator.CallDate;
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

        private EnumTypeItem GetLeadNegotiatorUserType()
        {
            return this.enumTypeItemRepository.FindBy(i => i.Code == EnumTypeItemCode.LeadNegotiator).Single();
        }

        private EnumTypeItem GetSecondaryNegotiatorUserType()
        {
            return this.enumTypeItemRepository.FindBy(i => i.Code == EnumTypeItemCode.SecondaryNegotiator).Single();
        }
    }
}
