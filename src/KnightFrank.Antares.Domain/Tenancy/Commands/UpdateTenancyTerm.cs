namespace KnightFrank.Antares.Domain.Tenancy.Commands
{
    using System;

    public class UpdateTenancyTerm
    {
        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}