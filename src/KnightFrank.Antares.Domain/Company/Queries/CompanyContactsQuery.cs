namespace KnightFrank.Antares.Domain.Company.Queries
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Company;

    using MediatR;

    public class CompanyContactsQuery : IRequest<IEnumerable<CompanyContact>>
    {

    }
}