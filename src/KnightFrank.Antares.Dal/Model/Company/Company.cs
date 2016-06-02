namespace KnightFrank.Antares.Dal.Model.Company
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Enum;

    public class Company : BaseEntity 
    {
        public string Name { get; set; }
        public string WebsiteUrl { get; set; }
        public string ClientCarePageUrl { get; set; }
        public Guid? ClientCareStatusId { get; set; }
        public EnumTypeItem ClientCareStatus { get; set; }
       // public virtual ICollection<Contact> Contacts { get; set; }
       public List<Contact> Contacts { get; set; } = new List<Contact>();
    }
}