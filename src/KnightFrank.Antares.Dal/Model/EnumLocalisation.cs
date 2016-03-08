namespace KnightFrank.Antares.Dal.Model
{
    public class EnumLocalisation : BaseEntity
    {
        public int EnumTypeItemId { get; set; }

        public virtual EnumTypeItem EnumTypeItem { get; set; }

        public int LocalId { get; set; }

        public virtual Local Local { get; set; }

        public string Value { get; set; }
    }
}
