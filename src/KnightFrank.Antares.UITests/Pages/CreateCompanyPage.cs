namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class CreateCompanyPage : ProjectPageBase
    {
        private readonly ElementLocator addContact = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator companyName = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, string.Empty);

        public CreateCompanyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ContactsListPage ContactsList => new ContactsListPage(this.DriverContext);

        public CreateCompanyPage OpenCreateCompanyPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("create company");
            return this;
        }

        public void AddContactToCompany()
        {
            this.Driver.GetElement(this.addContact).Click();
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
