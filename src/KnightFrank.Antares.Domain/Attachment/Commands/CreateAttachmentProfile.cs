namespace KnightFrank.Antares.Domain.Attachment.Commands
{
    using AutoMapper;

    using KnightFrank.Antares.Dal.Model.Attachment;

    public class CreateAttachmentProfile : Profile
    {
        protected override void Configure()
        {
            this.CreateMap<CreateAttachment, Attachment>();
        }
    }
}