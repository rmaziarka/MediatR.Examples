using System;

namespace KnightFrank.Antares.Dal.Model.PropertyType
{
    public class PropertyType : BaseEntity { 
        public Guid? ParentId { get; set; }

        public PropertyType Parent { get; set; }

        public string Code { get; set; }
    }
}
