namespace KnightFrank.Antares.Domain.Attachment.QueryHandlers
{
    using System.Data.Entity;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Attachment;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Attachment.Queries;

    using MediatR;
    public class AttachmentQueryHandler :IRequestHandler<AttachmentQuery, Attachment>
    {
        private readonly IReadGenericRepository<Attachment> attachmentRepository;

        public AttachmentQueryHandler(IReadGenericRepository<Attachment> attachmentRepository)
        {
            this.attachmentRepository = attachmentRepository;
        }

        public Attachment Handle(AttachmentQuery message)
        {
            return this.attachmentRepository
                .Get()
                .Include(r => r.User)
                .SingleOrDefault(r => r.Id == message.Id);
        }
    }
}