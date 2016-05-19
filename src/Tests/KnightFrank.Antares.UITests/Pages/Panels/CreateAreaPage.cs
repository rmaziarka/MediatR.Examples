namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class CreateAreaPage: ProjectPageBase
    {
        private readonly ElementLocator name = new ElementLocator(Locator.CssSelector, "#addAreaForm div.area-details:nth-of-type({0}) #name");
        private readonly ElementLocator size = new ElementLocator(Locator.CssSelector, "#addAreaForm div.area-details:nth-of-type({0}) #size");
        private readonly ElementLocator addArea = new ElementLocator(Locator.CssSelector, "#addAreaForm [ng-click *= 'addNewArea']");
        private readonly ElementLocator saveArea = new ElementLocator(Locator.CssSelector, "#area-add-button");

        public CreateAreaPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public CreateAreaPage SetAreaDetails(string areaName, double areaSize, int place)
        {
            if (place == 1)
            {
                this.Driver.SendKeys(this.name.Format(place), areaName);
                this.Driver.SendKeys(this.size.Format(place), areaSize);
            }
            else
            {
                this.Driver.GetElement(this.addArea).Click();
                this.Driver.SendKeys(this.name.Format(place), areaName);
                this.Driver.SendKeys(this.size.Format(place), areaSize);
            }
            return this;
        }

        public CreateAreaPage SaveArea()
        {
            this.Driver.GetElement(this.saveArea).Click();
            this.Driver.WaitForAngularToFinish();
            return this;
        }

    }
}
