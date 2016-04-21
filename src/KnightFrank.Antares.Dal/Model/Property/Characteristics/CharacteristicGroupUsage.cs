namespace KnightFrank.Antares.Dal.Model.Property.Characteristics
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Resource;

    public class CharacteristicGroupUsage:BaseEntity
    {
        public Guid CharacteristicGroupId { get; set; }
        public virtual CharacteristicGroup CharacteristicGroup { get; set; }
        public Guid PropertyTypeId { get; set; }
        public virtual PropertyType PropertyType { get; set; }
        public Guid CountryId { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<CharacteristicGroupItem> CharacteristicGroupItems { get; set; }
        public short Order { get; set; }
        public bool IsDisplayLabel { get; set; }
    }
}