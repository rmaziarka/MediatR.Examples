namespace KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    public class ActivityAppraisalMeetingAttendeesMapper : IActivityReferenceMapper<ActivityAttendee>
    {
        private readonly ICollectionValidator collectionValidator;

        public ActivityAppraisalMeetingAttendeesMapper(ICollectionValidator collectionValidator)
        {
            this.collectionValidator = collectionValidator;
        }

        public void ValidateAndAssign(ActivityCommandBase message, Activity activity)
        {
            if (message.AppraisalMeeting.Attendees.Count == 0)
            {
                activity.ActivityAttendees.Clear();
                return;
            }

            List<Guid> usersIds =
                message.AppraisalMeeting.Attendees.Where(x => x.UserId.HasValue).Select(x => x.UserId.Value).ToList();

            if (usersIds.Count > 0)
            {
                this.collectionValidator.CollectionIsUnique(usersIds, ErrorMessage.Activity_AppraisalMeetingAttendees_Not_Unique);
                this.collectionValidator.CollectionContainsAll(
                    activity.ActivityUsers.Select(x => x.UserId).ToList(),
                    usersIds,
                    ErrorMessage.Missing_Activity_Attendees_Id);
            }

            List<Guid> contactsIds =
                message.AppraisalMeeting.Attendees.Where(x => x.ContactId.HasValue).Select(x => x.ContactId.Value).ToList();

            if (contactsIds.Count > 0)
            {
                this.collectionValidator.CollectionIsUnique(contactsIds, ErrorMessage.Activity_AppraisalMeetingAttendees_Not_Unique);
                this.collectionValidator.CollectionContainsAll(
                    activity.Contacts.Select(x => x.Id).ToList(),
                    contactsIds,
                    ErrorMessage.Missing_Activity_Attendees_Id);
            }

            List<ActivityAttendee> attendeesToDelete = activity.ActivityAttendees
                .Where(x => x.UserId.HasValue && usersIds.Contains(x.UserId.Value) == false ||
                            x.ContactId.HasValue && contactsIds.Contains(x.ContactId.Value) == false)
                .ToList();

            foreach (ActivityAttendee attendeeToDelete in attendeesToDelete)
            {
                activity.ActivityAttendees.Remove(attendeeToDelete);
            }

            IEnumerable<Guid> newUsersIds = usersIds
                .Where(userId => activity.ActivityAttendees.Where(x => x.UserId.HasValue)
                                         .Select(x => x.UserId.Value)
                                         .Contains(userId) == false);
            foreach (Guid userId in newUsersIds)
            {
                activity.ActivityAttendees.Add(new ActivityAttendee { UserId = userId });
            }

            IEnumerable<Guid> newContactsIds = contactsIds
                .Where(contactId => activity.ActivityAttendees.Where(x => x.ContactId.HasValue)
                                            .Select(x => x.ContactId.Value)
                                            .Contains(contactId) == false);
            foreach (Guid contactId in newContactsIds)
            {
                activity.ActivityAttendees.Add(new ActivityAttendee { ContactId = contactId });
            }
        }
    }
}