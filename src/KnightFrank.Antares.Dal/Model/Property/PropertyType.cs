namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;
    using System.Collections.Generic;

    public class PropertyType : BaseEntity
    {
        public Guid? ParentId { get; set; }

        public virtual PropertyType Parent { get; set; }
        public virtual ICollection<PropertyType> Children { get; set; }

        public string Code { get; set; }
        
        public string EnumCode { get; set; }
    }
}
