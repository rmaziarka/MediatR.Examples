namespace KnightFrank.Antares.Dal.Model.Common
{
    using System;

    public interface ILocaled
    {
        Guid LocaleId { get; set; }
        Locale Locale { get; set; }
    }
}