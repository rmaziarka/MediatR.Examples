namespace KnightFrank.Antares.UITests.Steps.ActivityTabs
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.UITests.Pages;
    using KnightFrank.Antares.UITests.Pages.ActivityTabs;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class MarketingTabSteps
    {
        private readonly DriverContext driverContext;
        private readonly ViewActivityPage page;
        private readonly ScenarioContext scenarioContext;

        public MarketingTabSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new ViewActivityPage(this.driverContext);
            }
        }

        [When(@"User clicks edit marketing button on marketing tab on view activity page")]
        public void EditMarketing()
        {
            this.page.ViewMarketingTab.EditMarketing();
        }

        [When(@"User clicks save marketing button on marketing tab on view activity page")]
        public void SaveMarketing()
        {
            this.page.EditMarketingTab.SaveMarketing();
        }

        [When(@"User fills in marketing description on marketing tab on view activity page")]
        public void FillInMarketingDescription(Table table)
        {
            var details = table.CreateInstance<MarketingDetails>();

            this.page.EditMarketingTab
                .SetStrapLine(details.Strapline)
                .SetFullDescription(details.FullDescription)
                .SetLocationDescription(details.LocationDescription);
        }

        [When(@"User fills in advertising on marketing tab on view activity page")]
        public void FillInAdvertising(Table table)
        {
            var details = table.CreateInstance<MarketingDetails>();

            this.page.EditMarketingTab
                .SetAdvertisingNote(details.AdvertisingNote)
                .SetPrContent(details.PrContent);

            if (details.PublishToWeb.Equals("Yes"))
            {
                this.page.EditMarketingTab
                    .SetPublishToWeb()
                    .SetPortalToMarketOn(details.PortalToMarketOn);
            }
            if (details.PrPermitted.Equals("Yes"))
            {
                this.page.EditMarketingTab.SetPrPermitted();
            }
        }

        [When(@"User fills in sales boards on marketing tab on view activity page")]
        public void FillInSalesBoards(Table table)
        {
            var details = table.CreateInstance<MarketingDetails>();
            details.BoardRemovalDate = DateTime.UtcNow.ToString("dd-MM-yyyy");

            if (!string.IsNullOrEmpty(details.Type))
            {
                this.page.EditMarketingTab.SelectType(details.Type);
            }
            if (!string.IsNullOrEmpty(details.Status))
            {
                this.page.EditMarketingTab.SelectStatus(details.Status);
            }
            if (details.BoardUpToDate.Equals("Yes"))
            {
                this.page.EditMarketingTab.SetBoardUpToDate();
            }

            this.page.EditMarketingTab
                .SetBoardRemovalDate(details.BoardRemovalDate)
                .SetSpecialInstructions(details.SpecialInstructions);

            this.scenarioContext.Set(details, "MarketingDetails");
        }

        [Then(@"Marketing description details on marketing tab on view activity page are same as the following")]
        public void CheckMarketingDescription(Table table)
        {
            var details = table.CreateInstance<MarketingDetails>();

            Verify.That(this.driverContext,
                () => Assert.Equal(details.Strapline, this.page.ViewMarketingTab.StrapLine),
                () => Assert.Equal(details.FullDescription, this.page.ViewMarketingTab.FullDescription),
                () => Assert.Equal(details.LocationDescription, this.page.ViewMarketingTab.LocationDescription));
        }

        [Then(@"Advertising details on marketing tab on view activity page are same as the following")]
        public void CheckAdvertising(Table table)
        {
            var details = table.CreateInstance<MarketingDetails>();

            Verify.That(this.driverContext,
                () => Assert.Equal(details.PublishToWeb, this.page.ViewMarketingTab.PublishToWeb),
                () => Assert.Equal(details.AdvertisingNote, this.page.ViewMarketingTab.AdvertisingNote),
                () => Assert.Equal(details.PrPermitted, this.page.ViewMarketingTab.PrPermitted),
                () => Assert.Equal(details.PrContent, this.page.ViewMarketingTab.PrContent));

            if (details.PublishToWeb.Equals("Yes"))
            {
                Verify.That(this.driverContext,
                    () => Assert.Equal(details.PortalToMarketOn, this.page.ViewMarketingTab.PortalsToMarketOn.Single()));
            }
        }

        [Then(@"Sales boards details on marketing tab on view activity page are same as the following")]
        public void CheckSalesBoards(Table table)
        {
            var details = table.CreateInstance<MarketingDetails>();
            details.BoardRemovalDate = this.scenarioContext.ContainsKey("MarketingDetails")
                ? details.BoardRemovalDate = this.scenarioContext.Get<MarketingDetails>("MarketingDetails").BoardRemovalDate
                : details.BoardRemovalDate = "-";

            Verify.That(this.driverContext,
                () => Assert.Equal(details.Type, this.page.ViewMarketingTab.Type),
                () => Assert.Equal(details.Status, this.page.ViewMarketingTab.Status),
                () => Assert.Equal(details.BoardUpToDate, this.page.ViewMarketingTab.BoardUpToDate),
                () => Assert.Equal(details.BoardRemovalDate, this.page.ViewMarketingTab.BoardRemovalDate),
                () => Assert.Equal(details.SpecialInstructions, this.page.ViewMarketingTab.SpecialInstructions));
        }
    }
}
