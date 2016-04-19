namespace KnightFrank.Antares.Dal.Model.Property
{
    using System;

    using KnightFrank.Antares.Dal.Model.Property.Characteristics;

    public class PropertyCharacteristic : BaseEntity
    {
        public Guid PropertyId { get; set; }
        public Guid CharacteristicId { get; set; }
        public virtual Characteristic Characteristic { get; set; }
        public string Text { get; set; }
    }
}