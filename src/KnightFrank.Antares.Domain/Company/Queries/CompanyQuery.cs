namespace KnightFrank.Antares.Domain.Company.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model.Company;
 
    using MediatR;

    public class CompanyQuery : IRequest<Company>
    {
        public Guid Id { get; set; }
    }
}
