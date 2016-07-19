namespace KnightFrank.Antares.UITests.Pages.ActivityTabs
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewMarketingTabPage : ProjectPageBase
    {
        private readonly ElementLocator editMarketing = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'toggleMarketingTabMode']");
        // Marketing description
        private readonly ElementLocator strapLine = new ElementLocator(Locator.Id, "marketingStrapline");
        private readonly ElementLocator fullDescription = new ElementLocator(Locator.Id, "marketingFullDescription");
        private readonly ElementLocator locationDescription = new ElementLocator(Locator.Id, "marketingLocationDescription");
        // Advertising
        private readonly ElementLocator publishToWeb = new ElementLocator(Locator.CssSelector, "#advertisingPublishToWeb span");
        private readonly ElementLocator portalsToMarketOn = new ElementLocator(Locator.CssSelector, "#advertisingPortals .ng-binding");
        private readonly ElementLocator advertisingNote = new ElementLocator(Locator.Id, "advertisingNote");
        private readonly ElementLocator prPermitted = new ElementLocator(Locator.CssSelector, "#advertisingPrPermitted span");
        private readonly ElementLocator prContent = new ElementLocator(Locator.Id, "advertisingPrContent");
        // Sales boards
        private readonly ElementLocator type = new ElementLocator(Locator.Id, "salesBoardType");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "salesBoardStatus");
        private readonly ElementLocator boardUpToDate = new ElementLocator(Locator.CssSelector, "#salesBoardUpToDate span");
        private readonly ElementLocator boardRemovalDate = new ElementLocator(Locator.Id, "salesBoardRemovalDate");
        private readonly ElementLocator specialInstructions = new ElementLocator(Locator.Id, "salesBoardSpecialInstructions");

        public ViewMarketingTabPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string StrapLine => this.Driver.GetElement(this.strapLine).Text;

        public string FullDescription => this.Driver.GetElement(this.fullDescription).Text;

        public string LocationDescription => this.Driver.GetElement(this.locationDescription).Text;

        public string PublishToWeb => this.Driver.GetElement(this.publishToWeb).Text;

        public List<string> PortalsToMarketOn => this.Driver.GetElements(this.portalsToMarketOn).Select(el => el.Text).ToList();

        public string AdvertisingNote => this.Driver.GetElement(this.advertisingNote).Text;

        public string PrPermitted => this.Driver.GetElement(this.prPermitted).Text;

        public string PrContent => this.Driver.GetElement(this.prContent).Text;

        public string Type => this.Driver.GetElement(this.type).Text;

        public string Status => this.Driver.GetElement(this.status).Text;

        public string BoardUpToDate => this.Driver.GetElement(this.boardUpToDate).Text;

        public string BoardRemovalDate => this.Driver.GetElement(this.boardRemovalDate).Text;

        public string SpecialInstructions => this.Driver.GetElement(this.specialInstructions).Text;

        public ViewMarketingTabPage EditMarketing()
        {
            this.Driver.Click(this.editMarketing);
            this.Driver.WaitForAngularToFinish();
            return this;
        }
    }

    internal class MarketingDetails
    {
        public string Strapline { get; set; }
        
        public string FullDescription { get; set; } 
        
        public string LocationDescription { get; set; }

        public string PublishToWeb { get; set; }
        
        public string AdvertisingNote { get; set; }
        
        public string PrPermitted { get; set; }

        public string PrContent { get; set; }

        public string Type { get; set; }

        public string Status { get; set; }
        
        public string BoardUpToDate { get; set; }
        
        public string BoardRemovalDate { get; set; }
        
        public string SpecialInstructions { get; set; }

        public string PortalToMarketOn { get; set; }

    }
}
