namespace KnightFrank.Antares.Domain.User.QueryResults
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Enum;

    public class UserDataResult
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Country { get; set; }

        public EnumTypeItem Division { get; set; }

        public string DivisionCode { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
