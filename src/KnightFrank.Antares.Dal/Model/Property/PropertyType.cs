namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;

    public class PropertyType : BaseEntity { 
        public Guid? ParentId { get; set; }

        public virtual PropertyType Parent { get; set; }

        public string Code { get; set; }
    }
}
