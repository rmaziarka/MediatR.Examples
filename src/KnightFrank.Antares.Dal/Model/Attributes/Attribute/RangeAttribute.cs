namespace KnightFrank.Antares.Dal.Model.Attributes.Attribute
{
    using KnightFrank.Antares.Dal.Model.Attributes.Field;

    public class RangeAttribute : FormAttribute
    {
        public virtual Field Min { get; set; }

        public virtual Field Max { get; set; }
    }
}
