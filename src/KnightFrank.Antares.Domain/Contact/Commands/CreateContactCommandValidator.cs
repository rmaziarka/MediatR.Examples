namespace KnightFrank.Antares.Domain.Contact.Commands
{
    using FluentValidation;

    public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
    {
        public CreateContactCommandValidator()
        {
			this.RuleFor(x => x.Title).NotEmpty().Length(1, 128);
			this.RuleFor(x => x.FirstName).Length(0, 128);
            this.RuleFor(x => x.LastName).NotEmpty().Length(1, 128);

			this.RuleFor(x => x.MailingFormalSalutation).Length(1, 128);
			this.RuleFor(x => x.MailingSemiformalSalutation).Length(1, 128);
			this.RuleFor(x => x.MailingInformalSalutation).Length(0, 128);
			this.RuleFor(x => x.MailingPersonalSalutation).Length(0, 128);
			this.RuleFor(x => x.MailingEnvelopeSalutation).Length(1, 128);

			this.RuleFor(x => x.EventInviteSalutation).Length(0, 128);
			this.RuleFor(x => x.EventSemiformalSalutation).Length(0, 128);
			this.RuleFor(x => x.EventInformalSalutation).Length(0, 128);
			this.RuleFor(x => x.EventPersonalSalutation).Length(0, 128);
			this.RuleFor(x => x.EventEnvelopeSalutation).Length(0, 128);

            this.RuleFor(x => x.LeadNegotiator).NotNull().SetValidator(new ContactUserValidator());
        }
	}
}