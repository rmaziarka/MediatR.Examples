namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreateViewingPage : ProjectPageBase
    {
        private readonly ElementLocator attendees = new ElementLocator(Locator.Id, string.Empty);
        private readonly ElementLocator date = new ElementLocator(Locator.Id, "viewing-date");
        private readonly ElementLocator endTime = new ElementLocator(Locator.CssSelector, "#end-time input");
        private readonly ElementLocator invitationText = new ElementLocator(Locator.Id, "invitation-text");
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator postViewingComment = new ElementLocator(Locator.Id, "post-viewing-comment");
        private readonly ElementLocator saveViewing = new ElementLocator(Locator.CssSelector, "button[ng-click *= 'saveViewing']");
        private readonly ElementLocator startTime = new ElementLocator(Locator.CssSelector, "#start-time input");

        public CreateViewingPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public CreateViewingPage SetDate(string viewingDate)
        {
            this.Driver.SendKeys(this.date, viewingDate);
            return this;
        }

        public CreateViewingPage SelectStartTime(string viewingStartTime)
        {
            this.Driver.GetElement<Select>(this.startTime).SelectByText(viewingStartTime);
            return this;
        }

        public CreateViewingPage SelectEndTime(string viewingEndTime)
        {
            this.Driver.GetElement<Select>(this.endTime).SelectByText(viewingEndTime);
            return this;
        }

        public CreateViewingPage SelectAttendees()
        {
            this.Driver.GetElement(this.attendees).Click();
            return this;
        }

        public CreateViewingPage SetInvitation(string text)
        {
            this.Driver.SendKeys(this.invitationText, text);
            return this;
        }

        public CreateViewingPage SetPostViewingComment(string text)
        {
            this.Driver.SendKeys(this.postViewingComment, text);
            return this;
        }

        public ViewingDetailsPage SaveViewing()
        {
            this.Driver.GetElement(this.saveViewing).Click();
            return new ViewingDetailsPage(this.DriverContext);
        }

        public CreateViewingPage WaitForPanelToBeVisible()
        {
            this.Driver.WaitForElementToBeDisplayed(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }
    }
}
