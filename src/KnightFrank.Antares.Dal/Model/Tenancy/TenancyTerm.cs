namespace KnightFrank.Antares.Dal.Model.Tenancy
{
    using System;

    public class TenancyTerm : BaseEntity
    {
        public decimal AgreedRent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid TenancyId { get; set; }
    }
}