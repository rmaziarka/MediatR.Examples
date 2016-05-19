namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.Collections.Generic;
    using System.Linq;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class ViewingDetailsPage : ProjectPageBase
    {
        private readonly ElementLocator attendees = new ElementLocator(Locator.CssSelector, "div[id *= 'viewing-preview-attendee-item']");
        private readonly ElementLocator date = new ElementLocator(Locator.Id, "viewing-preview-date");
        private readonly ElementLocator details = new ElementLocator(Locator.CssSelector, ".slide-in .section-details:nth-of-type(1) .panel-item");
        private readonly ElementLocator editViewing = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'showViewingEdit']");
        private readonly ElementLocator invitationText = new ElementLocator(Locator.Id, "viewing-preview-invitation-text");
        private readonly ElementLocator postViewingComment = new ElementLocator(Locator.Id, "viewing-preview-post-viewing-comment");
        private readonly ElementLocator negotiator = new ElementLocator(Locator.Id, "viewing-preview-negotiator-details");
        private readonly ElementLocator viewLink = new ElementLocator(Locator.CssSelector, ".slide-in .section-details:nth-of-type(1) a");

        public ViewingDetailsPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string Details => this.Driver.GetElement(this.details).Text;

        public string Date => this.Driver.GetElement(this.date).Text;

        public string Negotiator => this.Driver.GetElement(this.negotiator).Text;

        public string InvitationText => this.Driver.GetElement(this.invitationText).Text;

        public string PostViewingComment => this.Driver.GetElement(this.postViewingComment).Text;

        public List<string> Attendees => this.Driver.GetElements(this.attendees).Select(el => el.Text).ToList();

        public ViewingDetailsPage EditViewing()
        {
            this.Driver.GetElement(this.editViewing).Click();
            return this;
        }

        public ViewingDetailsPage ClickViewLink()
        {
            this.Driver.GetElement(this.viewLink).Click();
            return this;
        }
    }
}
