namespace KnightFrank.Antares.Domain.Tenancy.Commands
{
    using System;

    public class CreateTenancyTerm
    {
        public Guid? Id { get; set; }

        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}