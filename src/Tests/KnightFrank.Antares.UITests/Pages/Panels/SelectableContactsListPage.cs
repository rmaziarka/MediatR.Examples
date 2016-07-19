namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class SelectableContactsListPage : ProjectPageBase
    {
        private readonly ElementLocator applyButton = new ElementLocator(Locator.CssSelector, ".slide-in #company-contact-apply-button");
        private readonly ElementLocator sidePanelLoader = new ElementLocator(Locator.CssSelector, ".slide-in.side-panel-loading");
        private readonly ElementLocator contact = new ElementLocator(Locator.XPath, "//div[@class = 'side-panel-content']//div[contains(text(),'{0}')]/../div[text() = '{1}']//ancestor::div[contains(@class, 'card-selectable')]");

        public SelectableContactsListPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public SelectableContactsListPage WaitForContactsListToLoad()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.sidePanelLoader, BaseConfiguration.MediumTimeout);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public SelectableContactsListPage SelectContact(string name, string company)
        {
            //TODO needs to be imporved when search will be available 
            this.Driver.GetElements(this.contact.Format(name, company)).Last().Click();
            return this;
        }

        public SelectableContactsListPage ApplyContact()
        {
            this.Driver.Click(this.applyButton);
            return this;
        }
    }
}
