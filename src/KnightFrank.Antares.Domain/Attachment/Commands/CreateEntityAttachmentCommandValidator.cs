namespace KnightFrank.Antares.Domain.Attachment.Commands
{
    using System;

    using FluentValidation;

    public class CreateEntityAttachmentCommandValidator : AbstractValidator<CreateEntityAttachmentCommand>
    {
        public CreateEntityAttachmentCommandValidator()
        {
            this.RuleFor(x => x.EntityId).NotEqual(Guid.Empty);
            this.RuleFor(x => x.Attachment).NotNull();
            this.RuleFor(x => x.Attachment).SetValidator(new CreateAttachmentValidator());
        } 
    }
}