namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewCompanyPage : ProjectPageBase
    {
        private readonly ElementLocator clientCarePage = new ElementLocator(Locator.CssSelector, "#clientCarePageUrl [name = 'url']");
        private readonly ElementLocator clientCareStatus = new ElementLocator(Locator.Id, "client-care-status");
        private readonly ElementLocator companyCategory = new ElementLocator(Locator.Id, "company-category");
        private readonly ElementLocator companyDescription = new ElementLocator(Locator.Id, "description");
        private readonly ElementLocator companyName = new ElementLocator(Locator.Id, "name");
        private readonly ElementLocator companyType = new ElementLocator(Locator.Id, "company-type");
        private readonly ElementLocator companyViewForm = new ElementLocator(Locator.CssSelector, "company-view");
        private readonly ElementLocator contactsList = new ElementLocator(Locator.CssSelector, "#list-contacts .ng-binding");
        private readonly ElementLocator editCompanyButton = new ElementLocator(Locator.Id, "company-edit-btn");
        private readonly ElementLocator isValid = new ElementLocator(Locator.Id, "valid");
        private readonly ElementLocator relationshipManager = new ElementLocator(Locator.Id, "relationship-manager");
        private readonly ElementLocator website = new ElementLocator(Locator.CssSelector, "#websiteUrl [name = 'url']");

        public ViewCompanyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string Website => this.Driver.GetElement(this.website).Text;

        public string CompanyName => this.Driver.GetElement(this.companyName).Text;

        public string ClientCarePage => this.Driver.GetElement(this.clientCarePage).Text;

        public string ClientCareStatus => this.Driver.GetElement(this.clientCareStatus).Text;

        public List<string> Contacts => this.Driver.GetElements(this.contactsList).Select(el => el.Text).ToList();

        public string CompanyType => this.Driver.GetElement(this.companyType).Text;

        public string CompanyDescription => this.Driver.GetElement(this.companyDescription).Text;

        public string RelationshipManager => this.Driver.GetElement(this.relationshipManager).Text;

        public string IsValid => this.Driver.GetElement(this.isValid).Text;

        public string CompanyCategory => this.Driver.GetElement(this.companyCategory).Text;

        public ViewCompanyPage OpenViewCompanyPageWithId(string id)
        {
            new CommonPage(this.DriverContext).NavigateToPageWithId("view company", id);
            return this;
        }

        public bool IsViewCompanyFormPresent()
        {
            return this.Driver.IsElementPresent(this.companyViewForm, BaseConfiguration.MediumTimeout);
        }

        public ViewCompanyPage EditCompany()
        {
            this.Driver.Click(this.editCompanyButton);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        internal class CompanyDetails
        {
            public string Name { get; set; }

            public string WebsiteUrl { get; set; }

            public string ClientCarePageUrl { get; set; }

            public string ClientCareStatus { get; set; }

            public string Description { get; set; }

            public string CompanyCategory { get; set; }

            public string CompanyType { get; set; }

            public string RelationshipManager { get; set; }

            public bool IsValid { get; set; }
        }
    }
}
