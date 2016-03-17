namespace KnightFrank.Antares.UITests.Pages
{
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ContactsListPage : ProjectPageBase
    {
        private readonly ElementLocator contact = new ElementLocator(Locator.XPath, "//label[normalize-space(text()) = '{0}']//input");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.CssSelector, "button[ng-click = 'vm.updateContacts()']");

        public ContactsListPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ContactsListPage SelectContact(string firstName, string surname)
        {
            string contactDetails = firstName + " " + surname;
            this.Driver.WaitForElementToBeDisplayed(this.contact.Format(contactDetails), BaseConfiguration.MediumTimeout);

            if (!this.Driver.GetElements(this.contact.Format(contactDetails)).First().Selected)
                this.Driver.GetElements(this.contact.Format(contactDetails)).First().Click();
            return this;
        }

        public void SaveContact()
        {
            this.Driver.GetElement(this.saveButton).Click();
        }
    }
}
