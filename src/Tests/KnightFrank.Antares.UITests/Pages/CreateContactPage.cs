namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    using OpenQA.Selenium;

    public class CreateContactPage : ProjectPageBase
    {
        private readonly ElementLocator contactFirstName = new ElementLocator(Locator.Id, "first-name");
        private readonly ElementLocator contactForm = new ElementLocator(Locator.Id, "addContactForm");
        private readonly ElementLocator contactLastName = new ElementLocator(Locator.Id, "last-name");
        private readonly ElementLocator contactTitle = new ElementLocator(Locator.Name, "searchText");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.CssSelector, "#addContactForm button");
        private readonly ElementLocator salutationsMailingsDropdown = new ElementLocator(Locator.XPath, "//select[@id='default-mailing-salutation-id']");
        private readonly ElementLocator salutationsEventsDropdown = new ElementLocator(Locator.XPath, "//select[@id='defaultEventSalutationId']");
        private readonly ElementLocator mailingsFormalInput = new ElementLocator(Locator.XPath, "//input[@id='mailing-formal-salutation']");
        private readonly ElementLocator mailingsSemiformalInput = new ElementLocator(Locator.XPath, "//input[@id='mailing-semiformal-salutation']");
        private readonly ElementLocator mailingsInformalInput = new ElementLocator(Locator.XPath, "//input[@id='mailing-informal-salutation']");
        private readonly ElementLocator mailingsPersonalInput = new ElementLocator(Locator.XPath, "//input[@id='mailing-personal-salutation']");
        private readonly ElementLocator mailingsEnvelopeInput = new ElementLocator(Locator.XPath, "//input[@id='mailing-envelope-salutation']");

        public CreateContactPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string Title => this.Driver.Value(this.contactTitle);

        public string FirstName => this.Driver.Value(this.contactFirstName);

        public string LastName => this.Driver.Value(this.contactLastName);

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

        public CreateContactPage SetTitle(string title)
        {
            this.Driver.SendKeys(this.contactTitle, title);
            return this;
        }

        public CreateContactPage SetFirstName(string firstName)
        {
            this.Driver.SendKeys(this.contactFirstName, firstName);
            return this;
        }

        public CreateContactPage SetLastName(string surname)
        {
            this.Driver.SendKeys(this.contactLastName, surname);
            return this;
        }

        public CreateContactPage SetMailingsUseSalutation(string salutation)
        {
            this.Driver.GetElement(this.salutationsMailingsDropdown).SendKeys(salutation);
            return this;
        }

        public CreateContactPage SetEventsUseSalutation(string salutation)
        {
            this.Driver.GetElement(this.salutationsEventsDropdown).SendKeys(salutation);
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

        public bool CheckIfContactAddPage()
        {
            string currentUrl = this.Driver.Url;
            return currentUrl == CommonPage.GetUrl("CreateContactPage").ToString();
        }
    }
}
