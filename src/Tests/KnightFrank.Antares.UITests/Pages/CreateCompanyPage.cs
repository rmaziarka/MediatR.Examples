namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class CreateCompanyPage : ProjectPageBase
    {
        private readonly ElementLocator addContact = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'showContactList']");
        private readonly ElementLocator companyName = new ElementLocator(Locator.Name, "name");
        private readonly ElementLocator contactsList = new ElementLocator(Locator.CssSelector, "#list-contacts .ng-binding");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.CssSelector, "button[type = 'submit']");

        public CreateCompanyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ContactsListPage ContactsList => new ContactsListPage(this.DriverContext);

        public List<string> Contacts => this.Driver.GetElements(this.contactsList).Select(el => el.Text).ToList();

        public CreateCompanyPage OpenCreateCompanyPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("create company");
            return this;
        }

        public void AddContactToCompany()
        {
            this.Driver.GetElement(this.addContact).Click();
            this.Driver.WaitForAngularToFinish();
        }

        public CreateCompanyPage SetCompanyName(string name)
        {
            this.Driver.SendKeys(this.companyName, name);
            return this;
        }

        public CreateCompanyPage SaveCompany()
        {
            this.Driver.GetElement(this.saveButton).Click();
            return this;
        }
    }
}
