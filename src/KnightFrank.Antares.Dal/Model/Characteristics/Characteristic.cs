namespace KnightFrank.Antares.Dal.Model.Characteristics
{
    public class Characteristic : BaseEntity
    {
        public bool Enabled { get; set; }
        public string Code { get; set; }
        public bool DisplayText { get; set; }
    }
}