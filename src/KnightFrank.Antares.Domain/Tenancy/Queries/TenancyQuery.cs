namespace KnightFrank.Antares.Domain.Tenancy.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model.Tenancy;

    using MediatR;
    public class TenancyQuery : IRequest<Tenancy>
    {
        public Guid Id { get; set; }
    }
}