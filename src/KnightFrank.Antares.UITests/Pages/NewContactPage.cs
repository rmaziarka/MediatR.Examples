namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class NewContactPage : ProjectPageBase
    {
        private readonly ElementLocator contactFirstName = new ElementLocator(Locator.Id, string.Empty);

        private readonly ElementLocator contactSurname = new ElementLocator(Locator.Id, string.Empty);

        private readonly ElementLocator contactTitle = new ElementLocator(Locator.Id, string.Empty);

        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, string.Empty);

        public NewContactPage(DriverContext driverContext)
            : base(driverContext)
        {
        }

        public NewContactPage OpenNewContactPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("New Contact");
            return this;
        }

        public NewContactPage SetTitle(string title)
        {
            this.Driver.GetElement(this.contactTitle).SendKeys(title);
            return this;
        }

        public NewContactPage SetFirstName(string firstName)
        {
            this.Driver.GetElement(this.contactFirstName).SendKeys(firstName);
            return this;
        }

        public NewContactPage SetSurname(string surname)
        {
            this.Driver.GetElement(this.contactSurname).SendKeys(surname);
            return this;
        }

        public NewContactPage SaveNewContact()
        {
            this.Driver.GetElement(this.saveButton).Click();
            return this;
        }
    }
}
