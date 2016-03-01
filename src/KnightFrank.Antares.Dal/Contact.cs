namespace KnightFrank.Antares.Dal
{
    using KnightFrank.Antares.Dal.Model;

    public class Contact : BaseEntity
    {
        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Title { get; set; }
    }
}
