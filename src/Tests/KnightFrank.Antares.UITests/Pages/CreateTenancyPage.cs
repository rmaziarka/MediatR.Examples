namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    using System.Collections.Generic;
    using System.Linq;

    public class CreateTenancyPage : ProjectPageBase
    {
        private readonly ElementLocator createTenancyForm = new ElementLocator(Locator.CssSelector, "tenancy-edit > div");
        private readonly ElementLocator activityDetails = new ElementLocator(Locator.CssSelector,"card[item *= 'tenancy.activity'] .ng-binding");
        private readonly ElementLocator requirementDetails = new ElementLocator(Locator.CssSelector,"card[item *= 'tenancy.requirement'] .ng-binding");
        private readonly ElementLocator tenants = new ElementLocator(Locator.CssSelector, "#tenancy-tenants-edit .panel-body .ng-binding");
        private readonly ElementLocator landlords = new ElementLocator(Locator.CssSelector, "#tenancy-landlords-edit .panel-body .ng-binding");
        private readonly ElementLocator startDate = new ElementLocator(Locator.Id, "termDateFrom");
        private readonly ElementLocator endDate = new ElementLocator(Locator.Id, "termDateTo");
        private readonly ElementLocator agreedRent = new ElementLocator(Locator.Id, "termAgreedRent");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.CssSelector, "button[type='submit']");

        public CreateTenancyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string RequirementDetails => this.Driver.GetElement(this.requirementDetails).Text;

        public List<string> TenancyTenants => this.Driver.GetElements(this.tenants).Select(x => x.Text).ToList();

        public List<string> TenancyLandlords => this.Driver.GetElements(this.landlords).Select(x => x.Text).ToList();

        public bool IsCreateTenancyPresent()
        {
            this.Driver.WaitForAngularToFinish();
            return this.Driver.IsElementPresent(this.createTenancyForm, BaseConfiguration.MediumTimeout);
        }

        public string GetActivityDetails()
        {
            List<string> list =
                this.Driver.GetElements(this.activityDetails, element => element.Enabled)
                    .Select(el => el.GetTextContent())
                    .ToList();
            return string.Join(" ", list).Trim();
        }

        public CreateTenancyPage SetStartDate(string start)
        {
            this.Driver.SendKeys(this.startDate, start);
            return this;
        }

        public CreateTenancyPage SetEndDate(string end)
        {
            this.Driver.SendKeys(this.endDate, end);
            return this;
        }

        public CreateTenancyPage SetAgreedRent(string terms)
        {
            this.Driver.SendKeys(this.agreedRent, terms);
            return this;
        }

        public CreateTenancyPage SaveTenancy()
        {
            this.Driver.Click(this.saveButton);
            return this;
        }
    }

    internal class TenancyDetails
    {
        public string Tenants { get; set; }

        public string Landlords { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string AggredRent { get; set; }

        public string Title { get; set; }

        public string Date { get; set; }
    }
}
