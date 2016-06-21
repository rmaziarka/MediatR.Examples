namespace KnightFrank.Antares.Domain.Attachment.Commands
{
    using System;

    using MediatR;

    public abstract class CreateEntityAttachmentCommand : IRequest<Guid>
    {
        public Guid EntityId { get; set; }

        public CreateAttachment Attachment { get; set; }
    }
}