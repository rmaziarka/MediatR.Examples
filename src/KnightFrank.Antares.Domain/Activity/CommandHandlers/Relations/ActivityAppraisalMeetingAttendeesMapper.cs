namespace KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    public class ActivityAppraisalMeetingAttendeesMapper : IActivityReferenceMapper<ActivityAttendee>
    {
        private readonly ICollectionValidator collectionValidator;
        private readonly IGenericRepository<ActivityAttendee> activityAttendeeRepository;
        private IEntityValidator entityValidator;

        public ActivityAppraisalMeetingAttendeesMapper(ICollectionValidator collectionValidator, IGenericRepository<ActivityAttendee> activityAttendeeRepository, IEntityValidator entityValidator)
        {
            this.collectionValidator = collectionValidator;
            this.activityAttendeeRepository = activityAttendeeRepository;
            this.entityValidator = entityValidator;
        }

        public void ValidateAndAssign(ActivityCommandBase message, Activity activity)
        {
            if (message.AppraisalMeetingAttendeesList.Count == 0)
            {
                activity.AppraisalMeetingAttendees.Clear();
                return;
            }

            List<Guid> usersIds =
                message.AppraisalMeetingAttendeesList.Where(x => x.UserId.HasValue).Select(x => x.UserId.Value).ToList();

            List<Guid> contactsIds =
                message.AppraisalMeetingAttendeesList.Where(x => x.ContactId.HasValue).Select(x => x.ContactId.Value).ToList();

            this.ValidateUsers(usersIds);
            this.ValidateContacts(activity, contactsIds);

            activity.AppraisalMeetingAttendees
                    .Where(x => IsRemovedFromExistingList(x, usersIds, contactsIds))
                    .ToList()
                    .ForEach(x => this.activityAttendeeRepository.Delete(x));

            usersIds.Where(id => IsNewlyAddedUser(id, activity.AppraisalMeetingAttendees))
                    .ToList()
                    .ForEach(id => activity.AppraisalMeetingAttendees.Add(new ActivityAttendee { UserId = id }));

            contactsIds.Where(id => IsNewlyAddedContact(id, activity.AppraisalMeetingAttendees))
                       .ToList()
                       .ForEach(id => activity.AppraisalMeetingAttendees.Add(new ActivityAttendee { ContactId = id }));
        }

        private static bool IsNewlyAddedUser(Guid id, ICollection<ActivityAttendee> activityAttendees)
        {
            return !activityAttendees.Where(x => x.UserId.HasValue)
                                     .Select(x => x.UserId.Value)
                                     .Contains(id);
        }

        private static bool IsNewlyAddedContact(Guid id, ICollection<ActivityAttendee> activityAttendees)
        {
            return !activityAttendees.Where(x => x.ContactId.HasValue)
                                     .Select(x => x.ContactId.Value)
                                     .Contains(id);
        }

        private static bool IsRemovedFromExistingList(ActivityAttendee activityAttendee, List<Guid> usersIds,
            List<Guid> contactsIds)
        {
            return activityAttendee.UserId.HasValue && !usersIds.Contains(activityAttendee.UserId.Value) ||
                   activityAttendee.ContactId.HasValue && !contactsIds.Contains(activityAttendee.ContactId.Value);
        }

        private void ValidateUsers(List<Guid> newIds)
        {
            if (newIds.Count > 0)
            {
                this.collectionValidator.CollectionIsUnique(newIds, ErrorMessage.Activity_AppraisalMeetingAttendees_Not_Unique);
                this.entityValidator.EntitiesExist<User>(newIds);
            }
        }

        private void ValidateContacts(Activity activity, List<Guid> newIds)
        {
            if (newIds.Count > 0)
            {
                List<Guid> existingIds = activity.Contacts.Select(x => x.Id).ToList();
                this.collectionValidator.CollectionIsUnique(newIds, ErrorMessage.Activity_AppraisalMeetingAttendees_Not_Unique);
                this.collectionValidator.CollectionContainsAll(
                    existingIds,
                    newIds,
                    ErrorMessage.Missing_Activity_Attendees_Id);
            }
        }
    }
}
