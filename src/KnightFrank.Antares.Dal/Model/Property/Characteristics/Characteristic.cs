namespace KnightFrank.Antares.Dal.Model.Property.Characteristics
{
    public class Characteristic : BaseEntity
    {
        public string Code { get; set; }
        public bool IsDisplayText { get; set; }
        public bool IsEnabled { get; set; }
    }
}