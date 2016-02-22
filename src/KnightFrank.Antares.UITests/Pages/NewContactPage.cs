using Objectivity.Test.Automation.Common;
using Objectivity.Test.Automation.Common.Extensions;
using Objectivity.Test.Automation.Common.Types;

namespace KnightFrank.Antares.UITests.Pages
{
    public class NewContactPage : ProjectPageBase
    {
        public NewContactPage(DriverContext driverContext) : base(driverContext)
        {
        }

        private readonly ElementLocator contactTitle = new ElementLocator(Locator.Id, "");
        private readonly ElementLocator contactFirstName = new ElementLocator(Locator.Id, "");
        private readonly ElementLocator contactSurname = new ElementLocator(Locator.Id, "");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "");

        public NewContactPage OpenNewContactPage()
        {
            new CommonPage(DriverContext).NavigateToPage("New Contact");
            return this;
        }

        public NewContactPage SetTitle(string title)
        {
            Driver.GetElement(contactTitle).SendKeys(title);
            return this;
        }

        public NewContactPage SetFirstName(string firstName)
        {
            Driver.GetElement(contactFirstName).SendKeys(firstName);
            return this;
        }

        public NewContactPage SetSurname(string surname)
        {
            Driver.GetElement(contactSurname).SendKeys(surname);
            return this;
        }

        public NewContactPage SaveNewContact()
        {
            Driver.GetElement(saveButton).Click();
            return this;
        }
    }
}
