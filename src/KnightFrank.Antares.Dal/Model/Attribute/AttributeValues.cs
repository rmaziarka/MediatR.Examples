namespace KnightFrank.Antares.Dal.Model.Attribute
{
    public class AttributeValues : BaseEntity
    {
        public int? MinBedrooms { get; set; }
        public int? MaxBedrooms { get; set; }
        public int? MinReceptions { get; set; }
        public int? MaxReceptions { get; set; }
        public int? MinBathrooms { get; set; }
        public int? MaxBathrooms { get; set; }
        public int? MinArea { get; set; }
        public int? MaxArea { get; set; }
        public int? MinLandArea { get; set; }
        public int? MaxLandArea { get; set; }
        public int? MinGuestRooms { get; set; }
        public int? MaxGuestRooms { get; set; }
        public int? MinFunctionRooms { get; set; }
        public int? MaxFunctionRooms { get; set; }
        public int? MinCarParkingSpaces { get; set; }
        public int? MaxCarParkingSpaces { get; set; }
    }
}
