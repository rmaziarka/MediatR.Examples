namespace KnightFrank.Antares.Dal.Model.Common
{
    using System;

    using KnightFrank.Antares.Dal.Model.Resource;

    public interface ILocaled
    {
        Guid LocaleId { get; set; }
        Locale Locale { get; set; }
    }
}