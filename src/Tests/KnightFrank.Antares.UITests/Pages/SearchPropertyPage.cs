namespace KnightFrank.Antares.UITests.Pages
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    using OpenQA.Selenium;

    public class SearchPropertyPage : ProjectPageBase
    {
        private readonly ElementLocator searchField = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator searchResult = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator viewSearchForm = new ElementLocator(Locator.Id, string.Empty);

        public SearchPropertyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public SearchPropertyPage SearchForProperty(string property)
        {
            this.Driver.SendKeys(this.searchField, property);
            this.Driver.SendKeys(this.searchField, Keys.Enter);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public SearchPropertyPage SelectPropertySearchResult()
        {
            this.Driver.Click(this.searchResult);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public bool IsViewSearchFormPresent()
        {
            return this.Driver.IsElementPresent(this.viewSearchForm, BaseConfiguration.MediumTimeout);
        }

        public SearchPropertyPage OpenSearchPropertyPage()
        {
            new CommonPage(this.DriverContext).NavigateToPage("search property");
            return this;
        }
    }
}
