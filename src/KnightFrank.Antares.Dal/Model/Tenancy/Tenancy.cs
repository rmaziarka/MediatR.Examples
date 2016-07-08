namespace KnightFrank.Antares.Dal.Model.Tenancy
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;

    public class Tenancy : BaseEntity
    {
        public Guid RequirementId { get; set; }
        public virtual Requirement Requirement{ get; set; }
        public Guid ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual ICollection<TenancyTerm> Terms { get; set; }
        public virtual ICollection<TenancyContact> Contacts { get; set; }
        public Guid TenancyTypeId { get; set; }
        public virtual TenancyType TenancyType { get; set; }
    }
}
