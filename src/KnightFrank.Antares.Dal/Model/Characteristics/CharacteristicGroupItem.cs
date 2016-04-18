namespace KnightFrank.Antares.Dal.Model.Characteristics
{
    using System;

    public class CharacteristicGroupItem:BaseEntity
    {
        public Guid CharacteristicId { get; set; }
        public virtual Characteristic Characteristic { get; set; }
        public short Order { get; set; }
    }
}