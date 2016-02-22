using System;
using KnightFrank.Antares.UITests.Extensions;
using Objectivity.Test.Automation.Common;
using Objectivity.Test.Automation.Common.Extensions;
using Objectivity.Test.Automation.Common.Types;

namespace KnightFrank.Antares.UITests.Pages
{
    public class GooglePage : ProjectPageBase
    {
        public GooglePage(DriverContext driverContext) : base(driverContext)
        {
        }

        private readonly ElementLocator searchBox = new ElementLocator(Locator.Name, "q");
        private readonly ElementLocator searchButton = new ElementLocator(Locator.Name, "btnG");
        private readonly ElementLocator results = new ElementLocator(Locator.Id, "ires");

        public GooglePage OpenGooglePage()
        {
            new CommonPage(DriverContext).NavigateToPage("Home");
            return this;
        }

        public GooglePage SetSearchCriteria(string criteria)
        {
            Driver.GetElement(searchBox).SendKeys(criteria);
            return this;
        }

        public GooglePage ClickSearchButton()
        {
            Driver.WaitForElementToBeDisplayed(searchButton.ToBy(), BaseConfiguration.MediumTimeout);
            Driver.GetElement(searchButton).Click();
            return this;
        }

        public bool CheckIfResultsAreDisplayed()
        {
            Driver.WaitForAjax();
            return Driver.IsElementPresent(results, BaseConfiguration.MediumTimeout);
        }
    }
}
