namespace KnightFrank.Antares.Domain.Property.Commands
{
    using System;

    public class CreateOrUpdatePropertyCharacteristic
    {
        public Guid CharacteristicId { get; set; }

        public string Text { get; set; }
    }
}