namespace KnightFrank.Antares.Domain.User.QueryResults
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Domain.Enum.QueryResults;

    public class UserDataResult
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public EnumQueryItemResult Division { get; set; }

        public string DivisionCode{ get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
