namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ResidentialSalesRequirementDetailsPage : ProjectPageBase
    {
        private readonly ElementLocator locationRequirements = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator propertyRequirements = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator applicants = new ElementLocator(Locator.Id, string.Empty);

        public ResidentialSalesRequirementDetailsPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public List<string> GetLocationRequirements()
        {
            return this.Driver.GetElements(this.locationRequirements).Select(el => el.Text).ToList();
        }

        public List<string> GetPropertyRequirements()
        {
            return this.Driver.GetElements(this.propertyRequirements).Select(el => el.Text).ToList();
        }

        public List<string> GetApplicants()
        {
            return this.Driver.GetElements(this.applicants).Select(el => el.Text).ToList();
        } 
    }
}
