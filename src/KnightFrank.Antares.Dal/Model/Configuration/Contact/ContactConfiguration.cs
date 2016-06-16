namespace KnightFrank.Antares.Dal.Model.Configuration.Contact
{
    using KnightFrank.Antares.Dal.Model.Contacts;

    internal sealed class ContactConfiguration : BaseEntityConfiguration<Contact>
    {
	    public ContactConfiguration()
	    {
			this.Property(p => p.Title).HasMaxLength(128).IsRequired();
			this.Property(p => p.FirstName).HasMaxLength(128);
			this.Property(p => p.LastName).HasMaxLength(128).IsRequired();

			this.Property(p => p.MailingFormalSalutation).HasMaxLength(128);
			this.Property(p => p.MailingSemiformalSalutation).HasMaxLength(128);
			this.Property(p => p.MailingInformalSalutation).HasMaxLength(128);
			this.Property(p => p.MailingPersonalSalutation).HasMaxLength(128);
			this.Property(p => p.MailingEnvelopeSalutation).HasMaxLength(128);

			this.Property(p => p.EventInviteSalutation).HasMaxLength(128);
			this.Property(p => p.EventSemiformalSalutation).HasMaxLength(128);
			this.Property(p => p.EventInformalSalutation).HasMaxLength(128);
			this.Property(p => p.EventPersonalSalutation).HasMaxLength(128);
			this.Property(p => p.EventEnvelopeSalutation).HasMaxLength(128);
		}
	}
}
