namespace KnightFrank.Antares.UITests.Pages.ActivityTabs
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class EditMarketingTabPage : ProjectPageBase
    {
        private readonly ElementLocator saveMarketing = new ElementLocator(Locator.CssSelector, ".active button[type = 'submit']");
        // Marketing description
        private readonly ElementLocator strapLine = new ElementLocator(Locator.Id, "marketingStrapline");
        private readonly ElementLocator fullDescription = new ElementLocator(Locator.Id, "marketingFullDescription");
        private readonly ElementLocator locationDescription = new ElementLocator(Locator.Id, "marketingLocationDescription");
        // Advertising
        private readonly ElementLocator publishToWeb = new ElementLocator(Locator.CssSelector, "#advertisingPublishToWeb [value = 'true']");
        private readonly ElementLocator portalToMarketOn = new ElementLocator(Locator.XPath, "//div[@id = 'advertisingPortals']//span[text() = '{0}']");
        private readonly ElementLocator advertisingNote = new ElementLocator(Locator.Id, "advertisingNote");
        private readonly ElementLocator prPermitted = new ElementLocator(Locator.CssSelector, "#advertisingPrPermitted [value = 'true']");
        private readonly ElementLocator prContent = new ElementLocator(Locator.Id, "advertisingPrContent");
        // Sales boards
        private readonly ElementLocator type = new ElementLocator(Locator.Id, "salesBoardType");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "salesBoardStatus");
        private readonly ElementLocator boardUpToDate = new ElementLocator(Locator.CssSelector, "#salesBoardUpToDate [value = 'true']");
        private readonly ElementLocator boardRemovalDate = new ElementLocator(Locator.Id, "salesBoardRemovalDate");
        private readonly ElementLocator specialInstructions = new ElementLocator(Locator.Id, "salesBoardSpecialInstructions");

        public EditMarketingTabPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public EditMarketingTabPage SetStrapLine(string text)
        {
            this.Driver.SendKeys(this.strapLine, text);
            return this;
        }

        public EditMarketingTabPage SetFullDescription(string text)
        {
            this.Driver.SendKeys(this.fullDescription, text);
            return this;
        }

        public EditMarketingTabPage SetLocationDescription(string text)
        {
            this.Driver.SendKeys(this.locationDescription, text);
            return this;
        }

        public EditMarketingTabPage SetAdvertisingNote(string text)
        {
            this.Driver.SendKeys(this.advertisingNote, text);
            return this;
        }

        public EditMarketingTabPage SetPrContent(string text)
        {
            this.Driver.SendKeys(this.prContent, text);
            return this;
        }

        public EditMarketingTabPage SetBoardRemovalDate(string text)
        {
            this.Driver.SendKeys(this.boardRemovalDate, text);
            return this;
        }

        public EditMarketingTabPage SetSpecialInstructions(string text)
        {
            this.Driver.SendKeys(this.specialInstructions, text);
            return this;
        }

        public EditMarketingTabPage SelectType(string text)
        {
            this.Driver.GetElement<Select>(this.type).SelectByText(text);
            return this;
        }

        public EditMarketingTabPage SelectStatus(string text)
        {
            this.Driver.GetElement<Select>(this.status).SelectByText(text);
            return this;
        }

        public EditMarketingTabPage SetPublishToWeb()
        {
            this.Driver.GetElement<Checkbox>(this.publishToWeb).TickCheckbox();
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public EditMarketingTabPage SetPortalToMarketOn(string text)
        {
            this.Driver.Click(this.portalToMarketOn.Format(text));
            return this;
        }

        public EditMarketingTabPage SetPrPermitted()
        {
            this.Driver.GetElement<Checkbox>(this.prPermitted).TickCheckbox();
            return this;
        }

        public EditMarketingTabPage SetBoardUpToDate()
        {
            this.Driver.GetElement<Checkbox>(this.boardUpToDate).TickCheckbox();
            return this;
        }

        public EditMarketingTabPage SaveMarketing()
        {
            this.Driver.Click(this.saveMarketing);
            this.Driver.WaitForAngularToFinish();
            return this;
        }
    }
}
