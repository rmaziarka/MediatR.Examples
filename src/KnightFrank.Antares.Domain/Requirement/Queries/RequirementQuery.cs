namespace KnightFrank.Antares.Domain.Requirement.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model;

    using MediatR;

    public class RequirementQuery : IRequest<Requirement>
    {
        public Guid Id { get; set; }
    }
}
