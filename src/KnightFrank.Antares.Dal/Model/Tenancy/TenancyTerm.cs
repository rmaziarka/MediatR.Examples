namespace KnightFrank.Antares.Dal.Model.Tenancy
{
    using System;

    public class TenancyTerm : BaseEntity
    {
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid TenancyId { get; set; }
    }
}