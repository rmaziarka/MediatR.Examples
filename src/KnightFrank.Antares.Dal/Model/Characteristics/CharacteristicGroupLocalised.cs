namespace KnightFrank.Antares.Dal.Model.Characteristics
{
    using System;

    using KnightFrank.Antares.Dal.Model.Resource;

    public class CharacteristicGroupLocalised : BaseEntity
    {
        public Guid CharacteristicGroupId { get; set; }
        public string Label { get; set; }
        public Guid LocaleId { get; set; }
        public virtual Locale Locale { get; set; }
    }
}