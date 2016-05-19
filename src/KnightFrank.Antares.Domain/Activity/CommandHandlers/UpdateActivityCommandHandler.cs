namespace KnightFrank.Antares.Domain.Activity.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Common;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    using MediatR;

    public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, Guid>
    {
        private readonly IGenericRepository<Activity> activityRepository;
        private readonly IGenericRepository<ActivityUser> activityUserRepository;
        private readonly IEntityValidator entityValidator;
        private readonly ICollectionValidator collectionValidator;

        public UpdateActivityCommandHandler(IGenericRepository<Activity> activityRepository,
            IGenericRepository<ActivityUser> activityUserRepository, IEntityValidator entityValidator, ICollectionValidator collectionValidator)
        {
            this.activityRepository = activityRepository;
            this.activityUserRepository = activityUserRepository;
            this.entityValidator = entityValidator;
            this.collectionValidator = collectionValidator;
        }

        public Guid Handle(UpdateActivityCommand message)
        {
            Activity activity = this.activityRepository.GetWithInclude(x => x.Id == message.Id, x => x.ActivityUsers).SingleOrDefault();
            this.entityValidator.EntityExists(activity, message.Id);

            this.entityValidator.EntityExists<User>(message.LeadNegotiatorId);

            var commandNegotiators = new List<ActivityUser>
            {
                new ActivityUser { UserId = message.LeadNegotiatorId, UserType = UserTypeEnum.LeadNegotiator }
            };

            this.entityValidator.EntitiesExist<User>(message.SecondaryNegotiatorIds);

            commandNegotiators.AddRange(
                message.SecondaryNegotiatorIds.Select(
                    n => new ActivityUser { UserId = n, UserType = UserTypeEnum.SecondaryNegotiator }));

            this.collectionValidator.CollectionIsUnique(commandNegotiators.Select(n => n.UserId).ToList(), ErrorMessage.Activity_Negotiators_Not_Unique);

            Mapper.Map(message, activity);

            // ReSharper disable once PossibleNullReferenceException
            List<ActivityUser> existingNegotiators = activity.ActivityUsers.ToList();

            existingNegotiators
                .Where(n => IsRemovedFromExistingList(n, commandNegotiators))
                .ToList()
                .ForEach(n => this.activityUserRepository.Delete(n));

            commandNegotiators
                .Where(n => IsNewlyAddedToExistingList(n, existingNegotiators))
                .ToList()
                .ForEach(n => activity.ActivityUsers.Add(n));

            commandNegotiators
                .Where(n => IsUpdated(n, existingNegotiators))
                .Select(n => new { newNagotiator = n, oldNegotiator = GetOldActivityUser(n, existingNegotiators) })
                .ToList()
                .ForEach(pair => UpdateNegotiator(pair.newNagotiator, pair.oldNegotiator));

            this.activityRepository.Save();

            return activity.Id;
        }

        private static void UpdateNegotiator(ActivityUser newNagotiator, ActivityUser oldNegotiator)
        {
            oldNegotiator.UserType = newNagotiator.UserType;
        }

        private static bool IsRemovedFromExistingList(
                ActivityUser existingActivityUser,
                IEnumerable<ActivityUser> activityUsers)
        {
            return !activityUsers.Select(x => x.UserId).Contains(existingActivityUser.UserId);
        }

        private static bool IsNewlyAddedToExistingList(
            ActivityUser activityUser,
            IEnumerable<ActivityUser> existingActivityUsers)
        {
            return !existingActivityUsers.Select(x => x.UserId).Contains(activityUser.UserId);
        }

        private static bool IsUpdated(
            ActivityUser activityUser,
            IEnumerable<ActivityUser> existingActivityUsers)
        {
            return !IsNewlyAddedToExistingList(activityUser, existingActivityUsers);
        }

        private static ActivityUser GetOldActivityUser(
            ActivityUser activityUser,
            IEnumerable<ActivityUser> existingActivityUsers)
        {
            return existingActivityUsers.SingleOrDefault(x => x.UserId == activityUser.UserId);
        }
    }
}