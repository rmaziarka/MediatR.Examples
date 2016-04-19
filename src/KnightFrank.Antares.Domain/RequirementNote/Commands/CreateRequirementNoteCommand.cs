namespace KnightFrank.Antares.Domain.RequirementNote.Commands
{
    using System;

    using MediatR;

    public class CreateRequirementNoteCommand : IRequest<Guid>
    {
        public Guid RequirementId { get; set; }

        public string Description { get; set; }
    }
}