namespace KnightFrank.Antares.Domain.Attachment.Queries
{
    using System;

    using KnightFrank.Antares.Dal.Model.Attachment;

    using MediatR;
    public class AttachmentQuery : IRequest<Attachment>
    {
        public Guid Id { get; set; }
    }
}