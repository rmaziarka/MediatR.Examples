namespace KnightFrank.Antares.Domain.Activity.CommandHandlers.Relations
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Activity.Commands;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;

    public class ActivityContactsMapper : IActivityReferenceMapper<Contact>
    {
        private readonly IGenericRepository<Contact> contactRepository;
        private readonly ICollectionValidator collectionValidator;

        public ActivityContactsMapper(IGenericRepository<Contact> contactRepository, ICollectionValidator collectionValidator)
        {
            this.contactRepository = contactRepository;
            this.collectionValidator = collectionValidator;
        }

        public void ValidateAndAssign(ActivityCommandBase message, Activity activity)
        {
            if (message.ContactIds.Count == 0)
            {
                activity.Contacts.Clear();
                return;
            }

            this.collectionValidator.CollectionIsUnique(message.ContactIds, ErrorMessage.Activity_Contacts_Not_Unique);

            List<Contact> messageContacts = this.contactRepository.FindBy(x => message.ContactIds.Contains(x.Id)).ToList();

            this.collectionValidator.CollectionContainsAll(
                messageContacts.Select(x => x.Id),
                message.ContactIds,
                ErrorMessage.Missing_Activity_Contacts_Id);

            foreach (Contact contactToDelete in activity.Contacts.Where(x => message.ContactIds.Contains(x.Id) == false).ToList())
            {
                this.contactRepository.Delete(contactToDelete);
            }

            foreach (Contact contactToAdd in messageContacts.Where(x => activity.Contacts.Select(c => c.Id).Contains(x.Id) == false))
            {
                activity.Contacts.Add(contactToAdd);
            }
        }
    }
}