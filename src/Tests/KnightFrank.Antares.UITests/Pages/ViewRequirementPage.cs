namespace KnightFrank.Antares.UITests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewRequirementPage : ProjectPageBase
    {
        private readonly ElementLocator loadingIndicator = new ElementLocator(Locator.CssSelector, "[ng-show *= 'isLoading']");
        private readonly ElementLocator requirementDate = new ElementLocator(Locator.CssSelector, "span[translate *= 'CREATEDDATE'] ~ span");
        private readonly ElementLocator locationRequirementsDetails = new ElementLocator(Locator.XPath, "//*[contains(@translate, 'LOCATION')]/..//span");
        private readonly ElementLocator propertyRequirementsDetails = new ElementLocator(Locator.XPath, "//*[contains(@translate, 'BASIC_REQUIREMENTS')]/..//div[contains(@class, 'ng-binding')]");
        private readonly ElementLocator propertyRequirementsDetailsDescription = new ElementLocator(Locator.XPath, "//*[contains(@translate, 'DESCRIPTION')]/../p");
        private readonly ElementLocator requirementApplicants = new ElementLocator(Locator.CssSelector, "div[ng-repeat *= 'contacts'] div");

        public ViewRequirementPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public ViewRequirementPage WaitForDetailsToLoad()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.loadingIndicator, BaseConfiguration.LongTimeout);
            return this;
        }

        public Dictionary<string, string> GetLocationRequirements()
        {
            List<string> locationDetails = this.Driver.GetElements(this.locationRequirementsDetails).Select(el => el.Text).ToList();

            List<string> keys = locationDetails.Select(el => el).Where((el, index) => index % 2 == 0).ToList();
            List<string> values = locationDetails.Select(el => el).Where((el, index) => index % 2 != 0).ToList();

            return keys.Zip(values, Tuple.Create).ToDictionary(pair => pair.Item1, pair => pair.Item2);
        }

        public Dictionary<string, string> GetPropertyRequirements()
        {
            List<string> propertyDetails = this.Driver.GetElements(this.propertyRequirementsDetails).Select(el => el.Text).ToList();

            List<string> keys = propertyDetails.Select(el => el).Where((el, index) => index % 2 == 0).ToList();
            List<string> values = propertyDetails.Select(el => el).Where((el, index) => index % 2 != 0).ToList();

            Dictionary<string, string> details = keys.Zip(values, Tuple.Create).ToDictionary(pair => pair.Item1, pair => pair.Item2);
            details.Add("Requirement description", this.Driver.GetElement(this.propertyRequirementsDetailsDescription).Text);

            return details;
        }

        public string GetRequirementCreateDate()
        {
            return this.Driver.GetElement(this.requirementDate).Text;
        }

        public List<string> GetApplicants()
        {
            return this.Driver.GetElements(this.requirementApplicants).Select(el => el.Text).ToList();
        } 
    }
}

