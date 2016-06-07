namespace KnightFrank.Antares.Domain.Attachment.Commands
{
    using System;

    using FluentValidation;

    public class CreateAttachmentValidator : AbstractValidator<CreateAttachment>
    {
        public CreateAttachmentValidator()
        {
            this.RuleFor(x => x.DocumentTypeId).NotEqual(Guid.Empty);
            this.RuleFor(x => x.ExternalDocumentId).NotEqual(Guid.Empty).NotNull();
            this.RuleFor(x => x.FileName).NotEmpty();
            this.RuleFor(x => x.Size).GreaterThanOrEqualTo(0);
        }
    }
}