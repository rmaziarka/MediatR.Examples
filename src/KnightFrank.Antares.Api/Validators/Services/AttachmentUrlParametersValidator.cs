﻿namespace KnightFrank.Antares.Api.Validators.Services
{
    using FluentValidation;

    using KnightFrank.Antares.Api.Models;

    public class AttachmentUrlParametersValidator : AbstractValidator<AttachmentUrlParameters>
    {
        public AttachmentUrlParametersValidator()
        {
            this.RuleFor(x => x.DocumentTypeId).NotEmpty();
            this.RuleFor(x => x.EntityReferenceId).NotEmpty();
            this.RuleFor(x => x.Filename).NotEmpty();
            this.RuleFor(x => x.LocaleIsoCode).NotEmpty();
        }
    }
}