namespace KnightFrank.Antares.Dal.Model
{
    using System;
    using System.Collections.Generic;

    public class EnumTypeItem: BaseEntity
    {
        public string Code { get; set; }

        public Guid EnumTypeId { get; set; }

        public virtual EnumType EnumType { get; set; }

        public virtual ICollection<EnumLocalised> EnumLocaliseds { get; set; }
    }
}
