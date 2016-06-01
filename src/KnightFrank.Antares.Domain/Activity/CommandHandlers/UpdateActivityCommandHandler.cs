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
            Activity activity = this.activityRepository.GetWithInclude(x => x.Id == message.Id, x => x.ActivityUsers).SingleOrDefault();

            this.entityValidator.EntityExists(activity, message.Id);

            this.enumTypeItemValidator.ItemExists(EnumType.ActivityStatus, message.ActivityStatusId);

            this.ValidateActivityTypeFromCommand(message, activity);

            List<ActivityUser> commandNegotiators = this.ValidateAndRetrieveNegotiatorsFromCommand(message);

            Mapper.Map(message, activity);

            this.ValidateActivityNegotiators(commandNegotiators, activity);
            this.UpdateActivityNegotiators(commandNegotiators, activity);

            this.activityRepository.Save();

            // ReSharper disable once PossibleNullReferenceException
            return activity.Id;
        }

        private void ValidateActivityTypeFromCommand(UpdateActivityCommand message, Activity activity)
        {
            this.entityValidator.EntityExists<ActivityType>(message.ActivityTypeId);

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

            var commandNegotiators = new List<ActivityUser>
                                         {
                                             new ActivityUser
                                                 {
                                                     UserId = message.LeadNegotiator.UserId,
                                                     UserType = this.GetLeadNegotiatorUserType(),
                                                     CallDate =  message.LeadNegotiator.CallDate
                                                 }
                                         };

            //Secondary negotiators
            this.entityValidator.EntitiesExist<User>(message.SecondaryNegotiators.Select(n => n.UserId).ToList());

            commandNegotiators.AddRange(
                message.SecondaryNegotiators.Select(
                    n => new ActivityUser { UserId = n.UserId, UserType = this.GetSecondaryNegotiatorUserType(), CallDate = n.CallDate }));

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
                              .ForEach(ValidateNegotiatorCallDate);

            commandNegotiators.Where(n => IsUpdated(n, existingNegotiators))
                              .Select(n => new { newNagotiator = n, oldNegotiator = GetOldActivityUser(n, existingNegotiators) })
                              .ToList()
                              .ForEach(pair =>
                              {
                                  if (pair.newNagotiator.CallDate != pair.oldNegotiator.CallDate)
                                  {
                                      ValidateNegotiatorCallDate(pair.newNagotiator);
                                  }
                              });
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

        private static void ValidateNegotiatorCallDate(ActivityUser negotiator)
        {
            if (negotiator.CallDate.HasValue && (negotiator.CallDate.Value.Date < DateTime.UtcNow.Date))
            {
                //throw new BusinessValidationException(ErrorMessage.Activity_Negotiator_CallDate_InPast);
            }
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
