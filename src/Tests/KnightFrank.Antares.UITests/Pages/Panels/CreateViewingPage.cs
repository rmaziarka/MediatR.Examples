namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.Collections.Generic;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    public class CreateViewingPage : ProjectPageBase
    {
        private readonly ElementLocator attendee = new ElementLocator(Locator.XPath, "//*[contains(@class, 'slide-in')]//div[contains(@ng-repeat, 'attendee')]//label[contains(text(), '{0}')]");
        private readonly ElementLocator date = new ElementLocator(Locator.CssSelector, ".slide-in #viewing-date");
        private readonly ElementLocator endTime = new ElementLocator(Locator.CssSelector, ".slide-in #end-time input");
        private readonly ElementLocator invitationText = new ElementLocator(Locator.CssSelector, ".slide-in #invitation-text");
        private readonly ElementLocator postViewingComment = new ElementLocator(Locator.CssSelector, ".slide-in #post-viewing-comment");
        private readonly ElementLocator saveViewing = new ElementLocator(Locator.CssSelector, ".slide-in button[ng-click *= 'save']");
        private readonly ElementLocator startTime = new ElementLocator(Locator.CssSelector, ".slide-in #start-time input");

        public CreateViewingPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public CreateViewingPage SetDate(string viewingDate)
        {
            this.Driver.SendKeys(this.date, viewingDate);
            return this;
        }

        public CreateViewingPage SetStartTime(string viewingStartTime)
        {
            this.Driver.SendKeys(this.startTime, viewingStartTime);
            return this;
        }

        public CreateViewingPage SetEndTime(string viewingEndTime)
        {
            this.Driver.SendKeys(this.endTime, viewingEndTime);
            return this;
        }

        public CreateViewingPage SelectAttendees(List<string> attendeesList)
        {
            foreach (string el in attendeesList)
            {
                this.Driver.Click(this.attendee.Format(el));
            }
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

        public CreateViewingPage SaveViewing()
        {
            this.Driver.Click(this.saveViewing);
            this.Driver.WaitForAngularToFinish();
            return this;
        }
    }
}
