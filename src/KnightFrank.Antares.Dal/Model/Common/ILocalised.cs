namespace KnightFrank.Antares.Dal.Model.Common
{
    using System;

    using KnightFrank.Antares.Dal.Model.Resource;

    public interface ILocalised : IBaseEntity
    {
        Guid ResourceId { get; set; }

        Guid LocaleId { get; set; }

        Locale Locale { get; set; }

        string Value { get; set; }
    }
}
