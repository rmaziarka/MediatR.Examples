namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;
    using System.Collections.Generic;

    public class PropertyType : BaseEntityWithCode
    {
        public Guid? ParentId { get; set; }

        public virtual PropertyType Parent { get; set; }
        public virtual ICollection<PropertyType> Children { get; set; }
    }
}
