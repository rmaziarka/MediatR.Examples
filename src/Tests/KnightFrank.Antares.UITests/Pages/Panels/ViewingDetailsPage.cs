namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewingDetailsPage : ProjectPageBase
    {
        private readonly ElementLocator attendees = new ElementLocator(Locator.CssSelector, "div[id *= 'viewing-preview-attendee-item']");
        private readonly ElementLocator date = new ElementLocator(Locator.Id, "viewing-preview-date");
        private readonly ElementLocator activity = new ElementLocator(Locator.Id, "viewing-preview-activity-details");
        private readonly ElementLocator editViewing = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'showViewingEdit']");
        private readonly ElementLocator invitationText = new ElementLocator(Locator.Id, "viewing-preview-invitation-text");
        private readonly ElementLocator postViewingComment = new ElementLocator(Locator.Id, "viewing-preview-post-viewing-comment");
        private readonly ElementLocator negotiator = new ElementLocator(Locator.Id, "viewing-preview-negotiator-details");
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");

        public ViewingDetailsPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string Activity => this.Driver.GetElement(this.activity).Text;

        public string Date => this.Driver.GetElement(this.date).Text;

        public string Negotiator => this.Driver.GetElement(this.negotiator).Text;

        public string InvitationText => this.Driver.GetElement(this.invitationText).Text;

        public string PostViewingComment => this.Driver.GetElement(this.postViewingComment).Text;

        public List<string> Attendees => this.Driver.GetElements(this.attendees).Select(el => el.Text).ToList();

        public CreateViewingPage EditViewing()
        {
            this.Driver.GetElement(this.editViewing).Click();
            return new CreateViewingPage(this.DriverContext);
        }

        public ViewingDetailsPage WaitForPanelToBeVisible()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }
    }
}
