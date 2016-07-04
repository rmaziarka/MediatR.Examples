namespace KnightFrank.Antares.Domain.Contact.Commands
{
    using System;

    using FluentValidation;

    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Common.Enums;

    public class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
    {
        private readonly IEnumTypeItemValidator enumTypeItemValidator;

        public UpdateContactCommandValidator(IEnumTypeItemValidator enumTypeItemValidator)
        {
            this.enumTypeItemValidator = enumTypeItemValidator;

            this.RuleFor(x => x.Id).NotEmpty();
            this.RuleFor(x => x.FirstName).NotEmpty().Length(1, 255);
            this.RuleFor(x => x.LastName).NotEmpty().Length(1, 255);
            this.RuleFor(x => x.Title).NotEmpty().Length(1, 255);

            this.RuleFor(x => x.MailingFormalSalutation).Length(1, 128);
            this.RuleFor(x => x.MailingSemiformalSalutation).Length(1, 128);
            this.RuleFor(x => x.MailingInformalSalutation).Length(0, 128);
            this.RuleFor(x => x.MailingPersonalSalutation).Length(0, 128);
            this.RuleFor(x => x.MailingEnvelopeSalutation).Length(1, 128);
            this.RuleFor(x => x.DefaultMailingSalutationId).Must(this.IsValidDefaultMailingSalutationId);

            this.RuleFor(x => x.EventInviteSalutation).Length(0, 128);
            this.RuleFor(x => x.EventSemiformalSalutation).Length(0, 128);
            this.RuleFor(x => x.EventInformalSalutation).Length(0, 128);
            this.RuleFor(x => x.EventPersonalSalutation).Length(0, 128);
            this.RuleFor(x => x.EventEnvelopeSalutation).Length(0, 128);
            this.RuleFor(x => x.DefaultEventSalutationId).Must(this.IsValidDefaultEventSalutationId);
        }

        private bool IsValidDefaultMailingSalutationId(Guid? enumItemId)
        {
            if (enumItemId != null)
            {
                this.enumTypeItemValidator.ItemExists(EnumType.MailingSalutation, (Guid)enumItemId);
            }
            return true;
        }

        private bool IsValidDefaultEventSalutationId(Guid? enumItemId)
        {
            if (enumItemId != null)
            {
                this.enumTypeItemValidator.ItemExists(EnumType.EventSalutation, (Guid)enumItemId);
            }
            return true;
        }
    }
}