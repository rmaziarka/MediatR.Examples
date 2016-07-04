namespace KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    public class ActivityAppraisalMeetingAttendeesMapper : IActivityReferenceMapper<ActivityAttendee>
    {
        private readonly ICollectionValidator collectionValidator;

        private readonly IGenericRepository<ActivityAttendee> activityAttendeeRepository;

        public ActivityAppraisalMeetingAttendeesMapper(ICollectionValidator collectionValidator, IGenericRepository<ActivityAttendee> activityAttendeeRepository)
        {
            this.collectionValidator = collectionValidator;
            this.activityAttendeeRepository = activityAttendeeRepository;
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

            this.Validate(activity, usersIds, contactsIds);

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

        private void Validate(Activity activity, List<Guid> usersIds, List<Guid> contactsIds)
        {
            this.ValidateCollection(usersIds, activity.ActivityUsers.Select(x => x.UserId).ToList());
            this.ValidateCollection(contactsIds, activity.Contacts.Select(x => x.Id).ToList());
        }

        private void ValidateCollection(List<Guid> newIds, List<Guid> existingIds)
        {
            if (newIds.Count > 0)
            {
                this.collectionValidator.CollectionIsUnique(newIds, ErrorMessage.Activity_AppraisalMeetingAttendees_Not_Unique);
                this.collectionValidator.CollectionContainsAll(
                    existingIds,
                    newIds,
                    ErrorMessage.Missing_Activity_Attendees_Id);
            }
        }
    }
}
