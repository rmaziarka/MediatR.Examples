using System.Collections.Generic;
using System.Linq;
using KnightFrank.Antares.Dal;

namespace KnightFrank.Antares.Domain
{
    public class TestService
    {
        public IEnumerable<ContactDTO> GetContacts()
        {
            var context = new KnightFrankContext();
            return context.Contacts.Select(x => new ContactDTO()
            {
                FirstName = x.FirstName,
                Surname = x.Surname,
                Title = x.Title
            });
        }
    }
}
