namespace KnightFrank.Antares.Dal.Model.Contacts
{
	using System;

	using KnightFrank.Antares.Dal.Model.Enum;

	public class Contact : BaseAuditableEntity
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

		public virtual EnumTypeItem DefaultMailingSalutation { get; set; }

		public string EventInviteSalutation { get; set; }

		public string EventSemiformalSalutation { get; set; }

		public string EventInformalSalutation { get; set; }

		public string EventPersonalSalutation { get; set; }

		public string EventEnvelopeSalutation { get; set; }

		public Guid? DefaultEventSalutationId { get; set; }

		public virtual EnumTypeItem DefaultEventSalutation { get; set; }
	}
}
