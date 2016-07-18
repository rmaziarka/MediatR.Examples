namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreateCompanyPage : ProjectPageBase
    {
        private readonly ElementLocator addContact = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'showContactList']");
        private readonly ElementLocator clientCarePage = new ElementLocator(Locator.Id, "clientcareurl");
        private readonly ElementLocator clientCareStatus = new ElementLocator(Locator.CssSelector, "#client-care-status > select");
        private readonly ElementLocator companyCategory = new ElementLocator(Locator.CssSelector, "#category > select");
        private readonly ElementLocator companyType = new ElementLocator(Locator.CssSelector, "#type > select");
        private readonly ElementLocator companyDescription = new ElementLocator(Locator.Id, "description");
        private readonly ElementLocator addRelationshipManagerButton = new ElementLocator(Locator.Id, "edit-btn");
        private readonly ElementLocator addRelationshipManagerInput = new ElementLocator(Locator.CssSelector, "#user-search input");
        private readonly ElementLocator addRelationshipManager = new ElementLocator(Locator.XPath, "//search[@id='user-search']//span[starts-with(., '{0}')]");
        private readonly ElementLocator validCheckbox = new ElementLocator(Locator.Id, "comapny-is-valid");
        private readonly ElementLocator companyForm = new ElementLocator(Locator.CssSelector, "company-add");
        private readonly ElementLocator companyName = new ElementLocator(Locator.Id, "name");
        private readonly ElementLocator contactsList = new ElementLocator(Locator.CssSelector, "#list-contacts .ng-binding");
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "company-save-btn");
        private readonly ElementLocator website = new ElementLocator(Locator.Id, "website");
        private readonly ElementLocator websiteUrlIcon = new ElementLocator(Locator.XPath, "//input[@id = 'website']//ancestor::div[1]//a[@name = 'url']");

        private string currentWindowHandler;

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

        public CreateCompanyPage AddContactToCompany()
        {
            this.Driver.GetElement(this.addContact).Click();
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public CreateCompanyPage SetCompanyName(string name)
        {
            this.Driver.SendKeys(this.companyName, name);
            return this;
        }

        public CreateCompanyPage SetWebsite(string websiteUrl)
        {
            this.Driver.SendKeys(this.website, websiteUrl);
            return this;
        }

        public CreateCompanyPage SetClientCarePage(string clientCarePageUrl)
        {
            this.Driver.SendKeys(this.clientCarePage, clientCarePageUrl);
            return this;
        }

        public CreateCompanyPage SelectClientCareStatus(string status)
        {
            this.Driver.GetElement<Select>(this.clientCareStatus).SelectByText(status);
            return this;
        }

        public CreateCompanyPage SelectCategory(string category)
        {
            this.Driver.GetElement<Select>(this.companyCategory).SelectByText(category);
            return this;
        }
        public CreateCompanyPage SelectCompanyType(string comType)
        {
            this.Driver.GetElement<Select>(this.companyType).SelectByText(comType);
            return this;
        }

        public CreateCompanyPage SelectRelationshipManager(string realtionshipManager)
        {
            this.Driver.Click(this.addRelationshipManagerButton);
            this.Driver.WaitForAngularToFinish();
            this.Driver.SendKeys(this.addRelationshipManagerInput, realtionshipManager);
            this.Driver.Click(this.addRelationshipManager.Format(realtionshipManager));
            return this;
        }

        public CreateCompanyPage SetDescription(string description)
        {
            this.Driver.SendKeys(this.companyDescription, description);
            return this;
        }

        public CreateCompanyPage SetValid(bool valid)
        {
            if ((valid && !this.Driver.GetElement(this.validCheckbox).Selected) || (!valid && this.Driver.GetElement(this.validCheckbox).Selected))
            {
                this.Driver.Click(this.validCheckbox);
            }

            return this;
        }

        public CreateCompanyPage SaveCompany()
        {
            this.Driver.GetElement(this.saveButton).Click();
            return this;
        }

        public bool IsAddCompanyFormPresent()
        {
            return this.Driver.IsElementPresent(this.companyForm, BaseConfiguration.MediumTimeout);
        }

        public CreateCompanyPage WaitForSidePanelToShow()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public CreateCompanyPage WaitForSidePanelToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }

        public CreateCompanyPage ClickOnWebsiteLink()
        {
            this.currentWindowHandler = this.Driver.CurrentWindowHandle;
            this.Driver.GetElement(this.websiteUrlIcon).Click();
            return this;
        }

        public bool CheckNewTab(string url)
        {
            ReadOnlyCollection<string> windowHandlers = this.Driver.WindowHandles;
            foreach (string handler in windowHandlers)
            {
                if (handler != this.currentWindowHandler)
                {
                    this.Driver.SwitchTo().Window(handler);
                    this.Driver.WaitForAjax();
                    break;
                }
            }

            string currentUrl = this.Driver.Url;
            this.Driver.Close();
            this.Driver.SwitchTo().Window(this.currentWindowHandler);
            string ah = $"http://{url}/";
            return ah.Equals(currentUrl);
        }
    }
}
