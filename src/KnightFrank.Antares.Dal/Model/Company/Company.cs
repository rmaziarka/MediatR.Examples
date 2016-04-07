namespace KnightFrank.Antares.Dal.Model.Company
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Contacts;

    public class Company : BaseEntity 
    {
        public string Name { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; }
    }
}