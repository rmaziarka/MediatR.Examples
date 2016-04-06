namespace KnightFrank.Antares.Dal.Model.Company
{
    using System.Collections;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Contacts;

    public class Company : BaseEntity 
    {
        public string Name { get; set; }

        public ICollection<Contact> Contacts { get; set; }
    }
}