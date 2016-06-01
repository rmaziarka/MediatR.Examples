namespace KnightFrank.Antares.UITests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Dal.Model.Enum;
    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    using OpenQA.Selenium;

    public class CreateCompanyPage : ProjectPageBase
    {
        private readonly ElementLocator companyForm = new ElementLocator(Locator.CssSelector, "company-add");
        private readonly ElementLocator addContact = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'showContactList']");
        private readonly ElementLocator companyName = new ElementLocator(Locator.Id, "name");
        private readonly ElementLocator website = new ElementLocator(Locator.Id, "website");
        private readonly ElementLocator clientCarePage = new ElementLocator(Locator.Id, "clientcareurl");
        private readonly ElementLocator clientCareStatus = new ElementLocator(Locator.Id, "client-care-status");
        private readonly ElementLocator contactsList = new ElementLocator(Locator.CssSelector, "#list-contacts .ng-binding");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "company-save-btn");
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator websiteUrlIcon = new ElementLocator(Locator.CssSelector, "a[name='url'] > i");

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

        public CreateCompanyPage SetClientCareUrl(string clientCarePageUrl)
        {
            this.Driver.SendKeys(this.clientCarePage, clientCarePageUrl);
            return this;
        }

        public CreateCompanyPage SetClientCareStatus()
        {
            //select the first in the drop down.
            var select = this.Driver.GetElement<Select>(this.clientCareStatus);
          //  IWebElement element = select.SelectElement().Options.Single(o => o.Text.Trim().Equals(clientCareStatus));
            select.SelectByIndex(1);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public CreateCompanyPage SaveCompany()
        {
            this.Driver.GetElement(this.saveButton).Click();
            return this;
        }

        public bool IsCompanyFormPresent()
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
                    break;
                }
            }

            string currentUrl = this.Driver.Url;
            this.Driver.Close();
            this.Driver.SwitchTo().Window(this.currentWindowHandler);
            var ah = $"http://{url}/";
            return ah.Equals(currentUrl);

        }

     
    }
}
