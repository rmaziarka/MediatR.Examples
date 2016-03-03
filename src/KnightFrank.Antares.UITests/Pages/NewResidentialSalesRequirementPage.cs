namespace KnightFrank.Antares.UITests.Pages
{
    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class NewResidentialSalesRequirementPage : ProjectPageBase
    {
        // Location requirements
        private readonly ElementLocator locationCountry = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator locationStreetName = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator locationPostCode = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator locationTown = new ElementLocator(Locator.Id, string.Empty);

        private readonly ElementLocator saveResidentialSalesRequirement = new ElementLocator(Locator.Id, string.Empty);

        public NewResidentialSalesRequirementPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public NewResidentialSalesRequirementPage OpenNewResidentialSalesRequirementPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("New Residential Sales Requirement");
            return this;
        }

        public NewResidentialSalesRequirementPage SelectCountry(string country)
        {
            this.Driver.GetElement<Select>(this.locationCountry).SelectByValue(country);
            return this;
        }

        public NewResidentialSalesRequirementPage SetStreetName(string streetName)
        {
            this.Driver.GetElement(this.locationStreetName).SendKeys(streetName);
            return this;
        }

        public NewResidentialSalesRequirementPage SetPostCode(string postCode)
        {
            this.Driver.GetElement(this.locationPostCode).SendKeys(postCode);
            return this;
        }

        public NewResidentialSalesRequirementPage SetTown(string town)
        {
            this.Driver.GetElement(this.locationTown).SendKeys(town);
            return this;
        }

        public void SaveNewResidentialSalesRequirement()
        {
            this.Driver.GetElement(this.saveResidentialSalesRequirement).Click();
        }
    }
}
