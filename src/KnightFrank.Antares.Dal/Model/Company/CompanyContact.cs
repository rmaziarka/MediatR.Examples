namespace KnightFrank.Antares.Dal.Model.Company
{
    using System;

    using KnightFrank.Antares.Dal.Model.Contacts;

    public class CompanyContact : BaseEntity
    {
        public Guid ContactId { get; set; }
        public Contact Contact { get; set; }
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
