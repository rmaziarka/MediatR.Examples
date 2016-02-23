namespace KnightFrank.Antares.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal;

    public class TestService
    {
        public IEnumerable<Contact> GetContacts()
        {
            var context = new KnightFrankContext();
            return context.Contacts.Select(x => new Contact { FirstName = x.FirstName, Surname = x.Surname, Title = x.Title });
        }
    }
}
