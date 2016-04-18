namespace KnightFrank.Antares.Dal.Model.Characteristics
{
    using System;

    public class PropertyCharacteristic : BaseEntity
    {
        public Guid PropertyId { get; set; }
        public Guid CharacteristicId { get; set; }
        public virtual Characteristic Characteristic { get; set; }
        public string Text { get; set; }
    }
}