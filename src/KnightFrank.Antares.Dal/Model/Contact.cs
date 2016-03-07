namespace KnightFrank.Antares.Dal.Model
{
    using System.Collections.Generic;

    public class Contact : BaseEntity
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Title { get; set; }

        public ICollection<Requirement> Requirements { get; set; }
    }
}
