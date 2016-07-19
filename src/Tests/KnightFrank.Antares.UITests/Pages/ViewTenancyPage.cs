namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    using System.Collections.Generic;
    using System.Linq;

    public class ViewTenancyPage : ProjectPageBase
    {
        private readonly ElementLocator viewTenancyForm = new ElementLocator(Locator.CssSelector, "tenancy-view > div");
        private readonly ElementLocator startDate = new ElementLocator(Locator.CssSelector, "date-view-control[date *= 'startDate'] .ng-binding");
        private readonly ElementLocator endDate = new ElementLocator(Locator.CssSelector, "date-view-control[date *= 'endDate'] .ng-binding");
        private readonly ElementLocator agreedRent = new ElementLocator(Locator.CssSelector, "price-view-control[price *= 'agreedRent'] .ng-binding");
        private readonly ElementLocator activityDetails = new ElementLocator(Locator.CssSelector, "card[item *= 'tenancy.activity'] .ng-binding");
        private readonly ElementLocator requirementDetails = new ElementLocator(Locator.CssSelector, "card[item *= 'tenancy.requirement'] .ng-binding");
        private readonly ElementLocator tenants = new ElementLocator(Locator.CssSelector, "#tenancy-tenants-view .panel-body .ng-binding");
        private readonly ElementLocator landlords = new ElementLocator(Locator.CssSelector, "#tenancy-landlords-view .panel-body .ng-binding");
        private readonly ElementLocator title = new ElementLocator(Locator.Id, "tenancy-page-header");
        private readonly ElementLocator requirementAction = new ElementLocator(Locator.CssSelector, "card[item *= 'requirement'] .card-menu-button");
        private readonly ElementLocator requirementDetailsLink = new ElementLocator(Locator.CssSelector, "card[item *= 'requirement'] a[ng-click *= 'action']");
        private readonly ElementLocator editButton = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'goToEdit']");

        public ViewTenancyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string RequirementDetails => this.Driver.GetElement(this.requirementDetails).Text;

        public List<string> TenancyTenants => this.Driver.GetElements(this.tenants).Select(x => x.Text).ToList();

        public List<string> TenancyLandlords => this.Driver.GetElements(this.landlords).Select(x => x.Text).ToList();

        public string Title => this.Driver.GetElement(this.title).Text;

        public ViewTenancyPage OpenViewTenancytPageWithId(string id)
        {
            new CommonPage(this.DriverContext).NavigateToPageWithId("view tenancy", id);
            return this;
        }

        public bool IsViewTenancyPresent()
        {
            this.Driver.WaitForAngularToFinish();
            return this.Driver.IsElementPresent(this.viewTenancyForm, BaseConfiguration.MediumTimeout);
        }

        public Dictionary<string, string> GetTerms()
        {
            var terms = new Dictionary<string, string>
            {
                { "startDate", this.Driver.GetElement(this.startDate).Text },
                { "endDate", this.Driver.GetElement(this.endDate).Text },
                { "agreedRent", this.Driver.GetElement(this.agreedRent).Text }
            };
            return terms;
        }

        public string GetActivityDetails()
        {
            List<string> list =
                this.Driver.GetElements(this.activityDetails, element => element.Enabled)
                    .Select(el => el.GetTextContent())
                    .ToList();
            return string.Join(" ", list).Trim();
        }

        public ViewTenancyPage OpenRequirementsDetails()
        {
            this.Driver.Click(this.requirementAction);
            this.Driver.Click(this.requirementDetailsLink);
            return this;
        }

        public ViewTenancyPage OpenEditTenancy()
        {
            this.Driver.Click(this.editButton);
            return this;
        }
    }
}
