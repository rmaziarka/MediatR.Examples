namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class GooglePage : ProjectPageBase
    {
        private readonly ElementLocator results = new ElementLocator(Locator.Id, "ires");

        private readonly ElementLocator searchBox = new ElementLocator(Locator.Name, "q");

        private readonly ElementLocator searchButton = new ElementLocator(Locator.Name, "btnG");

        public GooglePage(DriverContext driverContext)
            : base(driverContext)
        {
        }

        public GooglePage OpenGooglePage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("Home");
            return this;
        }

        public GooglePage SetSearchCriteria(string criteria)
        {
            this.Driver.GetElement(this.searchBox).SendKeys(criteria);
            return this;
        }

        public GooglePage ClickSearchButton()
        {
            this.Driver.WaitForElementToBeDisplayed(this.searchButton.ToBy(), BaseConfiguration.MediumTimeout);
            this.Driver.GetElement(this.searchButton).Click();
            return this;
        }

        public bool CheckIfResultsAreDisplayed()
        {
            this.Driver.WaitForAjax();
            return this.Driver.IsElementPresent(this.results, BaseConfiguration.MediumTimeout);
        }
    }
}
