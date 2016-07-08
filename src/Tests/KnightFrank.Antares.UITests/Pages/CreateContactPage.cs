namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreateContactPage : ProjectPageBase
    {
        private readonly ElementLocator contactForm = new ElementLocator(Locator.Id, "addContactForm");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "saveBtn");
        // Contact
        private readonly ElementLocator firstName = new ElementLocator(Locator.Id, "first-name");
        private readonly ElementLocator lastName = new ElementLocator(Locator.Id, "last-name");
        private readonly ElementLocator title = new ElementLocator(Locator.Id, "title");
        private readonly ElementLocator titleDropdown = new ElementLocator(Locator.XPath, "(//ul[@class='dropdown-menu ng-isolate-scope']//span[starts-with(., '{0}')])[1]");
        // Salutation
        private readonly ElementLocator salutationsMailingsDropdown = new ElementLocator(Locator.CssSelector, "#default-mailing-salutation-id select");
        private readonly ElementLocator mailingsFormalInput = new ElementLocator(Locator.Id, "mailing-formal-salutation");
        private readonly ElementLocator mailingsSemiformalInput = new ElementLocator(Locator.Id, "mailing-semiformal-salutation");
        private readonly ElementLocator mailingsInformalInput = new ElementLocator(Locator.Id, "mailing-informal-salutation");
        private readonly ElementLocator mailingsPersonalInput = new ElementLocator(Locator.Id, "mailing-personal-salutation");
        private readonly ElementLocator mailingsEnvelopeInput = new ElementLocator(Locator.Id, "mailing-envelope-salutation");
        // Secondary Negotiators
        private readonly ElementLocator secondaryNegotiatorAddButton = new ElementLocator(Locator.Id, "addItemBtn");
        private readonly ElementLocator secondaryNegotiatorSearch = new ElementLocator(Locator.CssSelector, "#secondary-search input");
        private readonly ElementLocator secondaryNegotiatorDropdown = new ElementLocator(Locator.XPath, "//search[@id='secondary-search']//span[starts-with(., '{0}')]");
        private readonly ElementLocator secondaryNegotiatorCloseButton = new ElementLocator(Locator.CssSelector, "#secondary-search button");


        public CreateContactPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string FormalMailingSalutation => this.Driver.Value(this.mailingsFormalInput);

        public string SemiformalMailingSalutation => this.Driver.Value(this.mailingsSemiformalInput);

        public string InformalMailingSalutation => this.Driver.Value(this.mailingsInformalInput);

        public string PersonalMailingSalutation => this.Driver.Value(this.mailingsPersonalInput);

        public string EnvelopeMailingSalutation => this.Driver.Value(this.mailingsEnvelopeInput);

        public CreateContactPage OpenCreateContactPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("create contact");
            return this;
        }

        public CreateContactPage SetTitle(string text)
        {
            this.Driver.SendKeys(this.title, text);
            this.Driver.WaitForAngularToFinish();
            this.Driver.WaitForElementToBeDisplayed(this.titleDropdown.Format(text), BaseConfiguration.MediumTimeout);
            this.Driver.Click(this.titleDropdown.Format(text));
            return this;
        }

        public CreateContactPage SetFirstName(string text)
        {
            this.Driver.SendKeys(this.firstName, text);
            return this;
        }

        public CreateContactPage SetLastName(string text)
        {
            this.Driver.SendKeys(this.lastName, text);
            return this;
        }

        public CreateContactPage SelectMailingsUseSalutation(string text)
        {
            this.Driver.GetElement<Select>(this.salutationsMailingsDropdown).SelectByText(text);
            return this;
        }

        public CreateContactPage SelectSecondaryNegotiator(string text)
        {
            this.Driver.Click(this.secondaryNegotiatorAddButton);
            this.Driver.WaitForAngularToFinish();
            this.Driver.SendKeys(this.secondaryNegotiatorSearch, text);
            this.Driver.WaitForAngularToFinish();
            this.Driver.WaitForElementToBeDisplayed(this.secondaryNegotiatorDropdown.Format(text), BaseConfiguration.MediumTimeout);
            this.Driver.Click(this.secondaryNegotiatorDropdown.Format(text));
            this.Driver.Click(this.secondaryNegotiatorCloseButton);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public CreateContactPage SaveContact()
        {
            this.Driver.Click(this.saveButton);
            return this;
        }

        public bool IsContactFormPresent()
        {
            return this.Driver.IsElementPresent(this.contactForm, BaseConfiguration.MediumTimeout);
        }
    }

    internal class Salutation
    {
        public string DefaultMailingSalutation { get; set; }

        public string MailingFormalSalutation { get; set; }

        public string MailingSemiformalSalutation { get; set; }

        public string MailingInformalSalutation { get; set; }

        public string MailingPersonalSalutation { get; set; }

        public string MailingEnvelopeSalutation { get; set; }
    }
}
