namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class EditCompanyPage : ProjectPageBase
    {
        private readonly ElementLocator addContact = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'showContactList']");
        private readonly ElementLocator clientCarePage = new ElementLocator(Locator.Id, "clientcareurl");
        private readonly ElementLocator clientCareStatus = new ElementLocator(Locator.CssSelector, "#client-care-status > select");
        private readonly ElementLocator companyForm = new ElementLocator(Locator.CssSelector, "company-edit");
        private readonly ElementLocator companyName = new ElementLocator(Locator.Id, "name");
        private readonly ElementLocator contactsList = new ElementLocator(Locator.CssSelector, "#list-contacts .ng-binding");
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "company-save-btn");
        private readonly ElementLocator website = new ElementLocator(Locator.Id, "website");

        public EditCompanyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ContactsListPage ContactsList => new ContactsListPage(this.DriverContext);

        public List<string> Contacts => this.Driver.GetElements(this.contactsList).Select(el => el.Text).ToList();

        public bool IsEditCompanyFormPresent()
        {
            return this.Driver.IsElementPresent(this.companyForm, BaseConfiguration.MediumTimeout);
        }

        public EditCompanyPage AddContactToCompany()
        {
            this.Driver.GetElement(this.addContact).Click();
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public EditCompanyPage SetCompanyName(string name)
        {
            this.Driver.SendKeys(this.companyName, name);
            return this;
        }

        public EditCompanyPage SetWebsite(string websiteUrl)
        {
            this.Driver.SendKeys(this.website, websiteUrl);
            return this;
        }

        public EditCompanyPage SetClientCareUrl(string clientCarePageUrl)
        {
            this.Driver.SendKeys(this.clientCarePage, clientCarePageUrl);
            return this;
        }

        public EditCompanyPage SelectClientCareStatus(string status)
        {
            this.Driver.GetElement<Select>(this.clientCareStatus).SelectByText(status);
            return this;
        }

        public EditCompanyPage SaveCompany()
        {
            this.Driver.GetElement(this.saveButton).Click();
            return this;
        }

        public EditCompanyPage WaitForSidePanelToShow()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public EditCompanyPage WaitForSidePanelToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }
    }
}
