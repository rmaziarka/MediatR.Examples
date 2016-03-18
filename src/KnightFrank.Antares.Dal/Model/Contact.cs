namespace KnightFrank.Antares.Dal.Model
{
    using System.Collections.Generic;

    public class Contact : BaseEntity
    {
        public Contact()
        {
	        this.Requirements = new List<Requirement>();
            this.Ownerships = new List<Ownership>();
        }

        public string FirstName { get; set; }

        public string Surname { get; set; }

        public string Title { get; set; }

        public ICollection<Requirement> Requirements { get; set; }

        public ICollection<Ownership> Ownerships { get; set; }

    }
}
