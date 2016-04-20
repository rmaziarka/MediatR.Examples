namespace KnightFrank.Antares.Dal.Model
{
    using System;
    using KnightFrank.Antares.Dal.Model.Common;
    using Resource;

    public abstract class BaseLocalisedEntity : BaseEntity, ILocalised
    {
        public Locale Locale { get; set; }

        public Guid LocaleId { get; set; }

        public Guid ResourceId { get; set; }

        public string Value { get; set; }
    }
}
