namespace KnightFrank.Antares.Dal.Model.Property.Activities
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Resource;

    public class ActivityTypeDefinition : BaseEntity
    {
        public Guid PropertyTypeId { get; set; }
        public virtual PropertyType PropertyType { get; set; }

        public Guid CountryId { get; set; }
        public virtual Country Country { get; set; }

        public Guid ActivityTypeId { get; set; }
        public virtual ActivityType ActivityType { get; set; }

        public short Order { get; set; }

        public virtual ICollection<RequirementType> RequirementTypes { get; set; }
    }
}
