namespace KnightFrank.Antares.Domain.Enum
{
    using System;
    using System.Collections.Generic;

    public class EnumQueryResult
    {
        public IEnumerable<EnumQueryItemResult> Items { get; set; }
    }

    public class EnumQueryItemResult
    {
        public Guid Id { get; set; }

        public string Value { get; set; }
    }
}