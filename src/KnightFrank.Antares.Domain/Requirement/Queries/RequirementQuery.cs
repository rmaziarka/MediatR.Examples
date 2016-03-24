namespace KnightFrank.Antares.Domain.Requirement.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model.Property;

    using MediatR;

    public class RequirementQuery : IRequest<Requirement>
    {
        public Guid Id { get; set; }
    }
}
