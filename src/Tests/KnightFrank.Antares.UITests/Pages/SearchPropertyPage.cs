namespace KnightFrank.Antares.UITests.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class SearchPropertyPage : ProjectPageBase
    {
        private readonly ElementLocator propertyAaddress = new ElementLocator(Locator.CssSelector, "card-list card-list-item:nth-of-type(1) .card-item .ng-binding");
        private readonly ElementLocator propertyOwnership = new ElementLocator(Locator.CssSelector, "card-list card-list-item:nth-of-type(1) .card-info");
        private readonly ElementLocator searchButton = new ElementLocator(Locator.CssSelector, ".search-property .input-group-btn");
        private readonly ElementLocator searchField = new ElementLocator(Locator.Id, "search-query");
        private readonly ElementLocator searchResult = new ElementLocator(Locator.CssSelector, ".search-property-results card-list-item:nth-of-type(1) .card-item .address-view");
        private readonly ElementLocator viewSearchForm = new ElementLocator(Locator.CssSelector, "property-search > div");

        public SearchPropertyPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string GetAddressDetails()
        {
            List<string> list = this.Driver.GetElements(this.propertyAaddress).Select(x => x.Text).ToList();
            return list.Aggregate<string, string>(null, (current, el) => current + (el + " ")).Trim();
        } 

        public string AddressOwnership => this.Driver.GetElement(this.propertyOwnership).Text;

        public SearchPropertyPage SearchProperty(string property)
        {
            this.Driver.SendKeys(this.searchField, property);
            this.Driver.Click(this.searchButton);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public SearchPropertyPage ClickSearchResult()
        {
            this.Driver.Click(this.searchResult);
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
