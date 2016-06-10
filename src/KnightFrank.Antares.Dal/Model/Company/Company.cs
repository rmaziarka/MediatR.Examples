namespace KnightFrank.Antares.Dal.Model.Company
{
    using System.Collections.Generic;

    public class Company : BaseEntity 
    {
        public string Name { get; set; }

        public virtual ICollection<CompanyContact> CompaniesContacts { get; set; }
    }
}