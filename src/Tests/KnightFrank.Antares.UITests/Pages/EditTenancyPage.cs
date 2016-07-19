namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;

    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class EditTenancyPage:ProjectPageBase
    {
        private readonly ElementLocator editTenancyForm = new ElementLocator(Locator.CssSelector, "tenancy-edit > div");
        private readonly ElementLocator activityDetails = new ElementLocator(Locator.CssSelector, "card[item *= 'tenancy.activity'] .ng-binding");
        private readonly ElementLocator requirementDetails = new ElementLocator(Locator.CssSelector, "card[item *= 'tenancy.requirement'] .ng-binding");
        private readonly ElementLocator startDate = new ElementLocator(Locator.Id, "termDateFrom");
        private readonly ElementLocator endDate = new ElementLocator(Locator.Id, "termDateTo");
        private readonly ElementLocator agreedRent = new ElementLocator(Locator.Id, "termAgreedRent");
        private readonly ElementLocator saveButton = new ElementLocator(Locator.CssSelector, "button[type='submit']");

        public EditTenancyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string RequirementDetails => this.Driver.GetElement(this.requirementDetails).Text;

        public bool IsEditTenancyPresent()
        {
            this.Driver.WaitForAngularToFinish();
            return this.Driver.IsElementPresent(this.editTenancyForm, BaseConfiguration.MediumTimeout);
        }

        public string GetActivityDetails()
        {
            List<string> list =
                this.Driver.GetElements(this.activityDetails, element => element.Enabled)
                    .Select(el => el.GetTextContent())
                    .ToList();
            return string.Join(" ", list).Trim();
        }

        public EditTenancyPage SetStartDate(string start)
        {
            this.Driver.SendKeys(this.startDate, start);
            return this;
        }

        public EditTenancyPage SetEndDate(string end)
        {
            this.Driver.SendKeys(this.endDate, end);
            return this;
        }

        public EditTenancyPage SetAgreedRent(string terms)
        {
            this.Driver.SendKeys(this.agreedRent, terms);
            return this;
        }

        public EditTenancyPage SaveTenancy()
        {
            this.Driver.Click(this.saveButton);
            return this;
        }
    }
}
