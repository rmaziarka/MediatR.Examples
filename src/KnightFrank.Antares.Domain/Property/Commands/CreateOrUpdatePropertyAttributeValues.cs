namespace KnightFrank.Antares.Domain.Property.Commands
{
    public class CreateOrUpdatePropertyAttributeValues
    {
        public int? MinBedrooms { get; set; }
        public int? MaxBedrooms { get; set; }
        public int? MinReceptions { get; set; }
        public int? MaxReceptions { get; set; }
        public int? MinBathrooms { get; set; }
        public int? MaxBathrooms { get; set; }
        public double? MinArea { get; set; }
        public double? MaxArea { get; set; }
        public double? MinLandArea { get; set; }
        public double? MaxLandArea { get; set; }
        public int? MinGuestRooms { get; set; }
        public int? MaxGuestRooms { get; set; }
        public int? MinFunctionRooms { get; set; }
        public int? MaxFunctionRooms { get; set; }
        public int? MinCarParkingSpaces { get; set; }
        public int? MaxCarParkingSpaces { get; set; }
    }
}
