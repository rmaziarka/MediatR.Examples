namespace KnightFrank.Antares.Dal.Model
{
    using System.Collections.Generic;

    public class EnumType: BaseEntity
    {
        public string Code { get; set; }

        public virtual ICollection<EnumTypeItem> EnumTypeItems { get; set; }
    }
}
