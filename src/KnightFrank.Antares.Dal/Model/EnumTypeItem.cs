namespace KnightFrank.Antares.Dal.Model
{
    using System.Collections.Generic;

    public class EnumTypeItem: BaseEntity
    {
        public string Code { get; set; }

        public int EnumTypeId { get; set; }

        public virtual EnumType EnumType { get; set; }

        public virtual ICollection<EnumLocalisation> EnumLocalisations { get; set; }
    }
}
