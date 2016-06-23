namespace KnightFrank.Antares.Dal.Model.Contacts
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Company;

	public class Contact : BaseEntity
    {
		public string Title { get; set; }

		public string FirstName { get; set; }

        public string Surname { get; set; }

		

        public virtual ICollection<CompanyContact> CompaniesContacts { get; set; }
	}
}
