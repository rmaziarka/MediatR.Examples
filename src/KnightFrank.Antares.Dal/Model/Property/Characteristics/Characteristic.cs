namespace KnightFrank.Antares.Dal.Model.Property.Characteristics
{
    public class Characteristic : BaseEntity
    {
        public string Code { get; set; }
        public bool DisplayText { get; set; }
        public bool Enabled { get; set; }
    }
}