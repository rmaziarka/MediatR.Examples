namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class NewContactPage : ProjectPageBase
    {
        private readonly ElementLocator contactFirstName = new ElementLocator(Locator.Id, "first-name");
        private readonly ElementLocator contactSurname = new ElementLocator(Locator.Id, "surname");
        private readonly ElementLocator contactTitle = new ElementLocator(Locator.Id, "title");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "saveBtn");

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
            this.Driver.SendKeys(this.contactTitle, title);
            return this;
        }

        public NewContactPage SetFirstName(string firstName)
        {
            this.Driver.SendKeys(this.contactFirstName, firstName);
            return this;
        }

        public NewContactPage SetSurname(string surname)
        {
            this.Driver.SendKeys(this.contactSurname, surname);
            return this;
        }

        public NewContactPage SaveNewContact()
        {
            this.Driver.GetElement(this.saveButton).Click();
            return this;
        }
    }
}
