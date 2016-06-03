namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class CreateContactPage : ProjectPageBase
    {
        private readonly ElementLocator contactFirstName = new ElementLocator(Locator.Id, "first-name");
        private readonly ElementLocator contactForm = new ElementLocator(Locator.Id, "addContactForm");
        private readonly ElementLocator contactSurname = new ElementLocator(Locator.Id, "surname");
        private readonly ElementLocator contactTitle = new ElementLocator(Locator.Id, "title");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.CssSelector, "#addContactForm button");

        public CreateContactPage(DriverContext driverContext) : base(driverContext)
        {
        }

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

        public CreateContactPage SetSurname(string surname)
        {
            this.Driver.SendKeys(this.contactSurname, surname);
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
}
