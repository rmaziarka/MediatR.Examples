namespace KnightFrank.Antares.Domain.Enum
{
    using System.Collections.Generic;

    public class EnumQueryResult
    {
        public IEnumerable<EnumQueryItemResult> Items { get; set; }
    }
}