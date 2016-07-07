namespace KnightFrank.Antares.Dal.Model.Tenancy
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;

    public class Tenancy : BaseEntity
    {
        public Guid RequirementId { get; set; }
        public virtual Requirement Requirement{ get; set; }
        public Guid ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual ICollection<TenancyTerm> Terms { get; set; }
        public virtual ICollection<Contact> Tenants { get; set; }
        public virtual ICollection<Contact> Landlords { get; set; }
    }
}
