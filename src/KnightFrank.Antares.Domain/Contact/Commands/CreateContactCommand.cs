namespace KnightFrank.Antares.Domain.Contact.Commands
{
    using System;

    using MediatR;

    public class CreateContactCommand : IRequest<Guid>
    {
		public string Title { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string MailingFormalSalutation { get; set; }

		public string MailingSemiformalSalutation { get; set; }

		public string MailingInformalSalutation { get; set; }

		public string MailingPersonalSalutation { get; set; }

		public string MailingEnvelopeSalutation { get; set; }

		public Guid? DefaultMailingSalutationId { get; set; }

		public string EventInviteSalutation { get; set; }

		public string EventSemiformalSalutation { get; set; }

		public string EventInformalSalutation { get; set; }

		public string EventPersonalSalutation { get; set; }

		public string EventEnvelopeSalutation { get; set; }

		public Guid? DefaultEventSalutationId { get; set; }
	}
}
