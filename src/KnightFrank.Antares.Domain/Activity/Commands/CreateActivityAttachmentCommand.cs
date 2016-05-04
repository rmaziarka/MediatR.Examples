namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    using KnightFrank.Antares.Domain.Attachment.Commands;

    using MediatR;

    public class CreateActivityAttachmentCommand : IRequest<Guid>
    {
        public Guid ActivityId { get; set; }

        public CreateAttachment Attachment { get; set; }
    }
}