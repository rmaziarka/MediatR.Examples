namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    using OpenQA.Selenium;

    public class ViewCompanyPage : ProjectPageBase
    {
        private readonly ElementLocator companyViewForm = new ElementLocator(Locator.CssSelector, "company-view");
        private readonly ElementLocator companyName = new ElementLocator(Locator.CssSelector, "div#name");
        private readonly ElementLocator website = new ElementLocator(Locator.CssSelector ,"#websiteUrl a[name=url]");
        private readonly ElementLocator clientCarePage = new ElementLocator(Locator.CssSelector, "#clientCarePageUrl a[name=url]");
        private readonly ElementLocator contactsList = new ElementLocator(Locator.CssSelector, "#list-contacts .ng-binding");
        private readonly ElementLocator editCompanyButton = new ElementLocator(Locator.Id, "company-edit-btn");

		public ViewCompanyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ContactsListPage ContactsList => new ContactsListPage(this.DriverContext);

        public List<string> Contacts => this.Driver.GetElements(this.contactsList).Select(el => el.Text).ToList();

        public bool IsViewCompanyFormPresent()
        {
            return this.Driver.IsElementPresent(this.companyViewForm, BaseConfiguration.MediumTimeout);
        }

        public string GetCompanyName()
        {
            IWebElement element = this.Driver.GetElement(this.companyName);
            return element.Text;
        }
    
		public string GetWebsiteUrl()
        {
            return this.Driver.GetElement(this.website).Text;
        }

		public string GetClientCareUrl()
        {
            return this.Driver.GetElement(this.clientCarePage).Text;
        }

	    public ViewCompanyPage EditCompany()
	    {
			this.Driver.GetElement(this.editCompanyButton).Click();
			return this;
		}
	}
}
