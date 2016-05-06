﻿namespace KnightFrank.Antares.UITests.Pages.Panels
{
    using System.Collections.Generic;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;

    using OpenQA.Selenium;

    public class CreateViewingPage : ProjectPageBase
    {
        private readonly ElementLocator panel = new ElementLocator(Locator.CssSelector, ".side-panel.slide-in");
        private readonly ElementLocator attendees = new ElementLocator(Locator.CssSelector, "div[ng-repeat *= 'attendee'] label");
        private readonly ElementLocator date = new ElementLocator(Locator.Id, "viewing-date");
        private readonly ElementLocator endTime = new ElementLocator(Locator.CssSelector, "#end-time input");
        private readonly ElementLocator invitationText = new ElementLocator(Locator.Id, "invitation-text");
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
            IList<IWebElement> list = this.Driver.GetElements(this.attendees);
            foreach (IWebElement el in list)
            {
                foreach (string attendee in attendeesList)
                {
                    if (el.Text.Equals(attendee))
                    {
                        el.Click();
                    }
                }
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
            this.Driver.GetElement(this.saveViewing).Click();
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public CreateViewingPage WaitForViewingDetailsToHide()
        {
            this.Driver.WaitUntilElementIsNoLongerFound(this.panel, BaseConfiguration.MediumTimeout);
            return this;
        }
    }
}
