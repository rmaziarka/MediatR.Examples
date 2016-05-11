namespace KnightFrank.Antares.Domain.Activity.Commands
{
    using System;

    using FluentValidation;

    using KnightFrank.Antares.Domain.Attachment.Commands;

    public class CreateActivityAttachmentCommandValidator : AbstractValidator<CreateActivityAttachmentCommand>
    {
        public CreateActivityAttachmentCommandValidator()
        {
            this.RuleFor(x => x.ActivityId).NotEqual(Guid.Empty);
            this.RuleFor(x => x.Attachment).NotNull();
            this.RuleFor(x => x.Attachment).SetValidator(new CreateAttachmentValidator());
        } 
    }
}