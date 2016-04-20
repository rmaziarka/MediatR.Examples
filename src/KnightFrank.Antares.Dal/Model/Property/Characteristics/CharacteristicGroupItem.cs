namespace KnightFrank.Antares.Dal.Model.Property.Characteristics
{
    using System;

    public class CharacteristicGroupItem : BaseEntity
    {
        public Guid CharacteristicId { get; set; }

        public virtual Characteristic Characteristic { get; set; }

        public Guid CharacteristicGroupUsageId { get; set; }

        public virtual CharacteristicGroupUsage CharacteristicGroupUsage { get; set; }
    }
}
