namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewContactPage : ProjectPageBase
    {
        private readonly ElementLocator name = new ElementLocator(Locator.Id, "name");
        private readonly ElementLocator viewContactForm = new ElementLocator(Locator.CssSelector, "contact-view > div");
        // Salutations
        private readonly ElementLocator defaultSalutation = new ElementLocator(Locator.Id, "defaultMailingSalutation");
        private readonly ElementLocator formal = new ElementLocator(Locator.Id, "mailingFormalSalutation");
        private readonly ElementLocator semiformal = new ElementLocator(Locator.Id, "mailingSemiformalSalutation");
        private readonly ElementLocator informal = new ElementLocator(Locator.Id, "mailingInformalSalutation");
        private readonly ElementLocator personal = new ElementLocator(Locator.Id, "mailingPersonalSalutation");
        private readonly ElementLocator envelope = new ElementLocator(Locator.Id, "mailingEnvelopeSalutation");

        public ViewContactPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string Name => this.Driver.GetElement(this.name).Text;

        public string DefaultMailingSalutation => this.Driver.GetElement(this.defaultSalutation).Text;

        public string MailingFormalSalutation => this.Driver.GetElement(this.formal).Text;

        public string MailingSemiformalSalutation => this.Driver.GetElement(this.semiformal).Text;

        public string MailingInformalSalutation => this.Driver.GetElement(this.informal).Text;

        public string MailingPersonalSalutation => this.Driver.GetElement(this.personal).Text;

        public string MailingEnvelopeSalutation => this.Driver.GetElement(this.envelope).Text;

        public bool IsViewContactFormPresent()
        {
            return this.Driver.IsElementPresent(this.viewContactForm, BaseConfiguration.MediumTimeout);
        }
    }
}
