namespace KnightFrank.Antares.Domain.RequirementNote.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model.Property;

    using MediatR;

    public class RequirementNoteQuery : IRequest<RequirementNote>
    {
        public Guid Id { get; set; }
    }
}
