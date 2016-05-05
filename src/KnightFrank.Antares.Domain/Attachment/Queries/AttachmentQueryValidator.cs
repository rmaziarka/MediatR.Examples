﻿namespace KnightFrank.Antares.Domain.Attachment.Queries
{
    using FluentValidation;
    public class AttachmentQueryValidator : AbstractValidator<AttachmentQuery>
    {
        public AttachmentQueryValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty();
        }
    }
}