namespace KnightFrank.Antares.UITests.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.UITests.Extensions;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;
    using Objectivity.Test.Automation.Common.Types;
    using Objectivity.Test.Automation.Common.WebElements;

    public class CreateActivityPage : ProjectPageBase
    {
        private readonly ElementLocator saveButton = new ElementLocator(Locator.Id, "activity-edit-save");
        private readonly ElementLocator status = new ElementLocator(Locator.Id, "activityStatusId");
        private readonly ElementLocator type = new ElementLocator(Locator.Id, "type");
        private readonly ElementLocator vendor = new ElementLocator(Locator.CssSelector, "#activity-vendors-edit span.ng-binding");
        // negotiators & departments locators
        private readonly ElementLocator negotiator = new ElementLocator(Locator.Id, "negotiator-");
        private readonly ElementLocator title = new ElementLocator(Locator.CssSelector, ".well > div:nth-of-type(1)");
        private readonly ElementLocator department = new ElementLocator(Locator.CssSelector, ".department-name");
        // property locators
        private readonly ElementLocator propertyNumber = new ElementLocator(Locator.CssSelector, "#activity-edit-basic .card-details ng-include > div:nth-of-type(1) >div:nth-of-type(1) span");
        private readonly ElementLocator propertyName = new ElementLocator(Locator.CssSelector, "#activity-edit-basic .card-details ng-include > div:nth-of-type(1) >div:nth-of-type(2) span");
        private readonly ElementLocator propertyLine2 = new ElementLocator(Locator.CssSelector, "#activity-edit-basic .card-details ng-include > div:nth-of-type(2) span");
        private readonly ElementLocator propertyPostCode = new ElementLocator(Locator.CssSelector, "#activity-edit-basic .card-details ng-include > div:nth-of-type(4) span");
        private readonly ElementLocator propertyCity = new ElementLocator(Locator.CssSelector, "#activity-edit-basic .card-details ng-include > div:nth-of-type(5) span");
        private readonly ElementLocator propertyCounty = new ElementLocator(Locator.CssSelector, "#activity-edit-basic .card-details ng-include > div:nth-of-type(6) span");
        // attendees locators
        private readonly ElementLocator attendees = new ElementLocator(Locator.CssSelector, "activity-attendees-edit-control label");
        private readonly ElementLocator attendeeToSelect = new ElementLocator(Locator.XPath, "//activity-attendees-edit-control//label[contains(.,'{0}')]/input");
        // basic information locators
        private readonly ElementLocator source = new ElementLocator(Locator.Id, "sourceId");
        private readonly ElementLocator sourceDescription = new ElementLocator(Locator.Id, "sourceDescriptionId");
        private readonly ElementLocator sellingReason = new ElementLocator(Locator.Id, "sellingReasonId");
        private readonly ElementLocator pitchingThreats = new ElementLocator(Locator.Id, "pitchingThreatsId");
        // additional information locators
        private readonly ElementLocator keyNumber = new ElementLocator(Locator.Id, "keyNumberId");
        private readonly ElementLocator accessArrangements = new ElementLocator(Locator.Id, "accessArrangementsId");
        // appraisal meeting locators
        private readonly ElementLocator date = new ElementLocator(Locator.Name, "meetingDate");
        private readonly ElementLocator startTime = new ElementLocator(Locator.CssSelector, "#meeting-start-time input");
        private readonly ElementLocator endTime = new ElementLocator(Locator.CssSelector, "#meeting-end-time input");
        private readonly ElementLocator invitationText = new ElementLocator(Locator.Id, "invitationTextId");

        public CreateActivityPage(DriverContext driverContext) : base(driverContext)
        {
        }

        public string ActivityVendor => this.Driver.GetElement(this.vendor).Text;

        public string ActivityNegotiator => this.Driver.GetElement(this.negotiator).Text;

        public string ActivityTitle => this.Driver.GetElement(this.title).Text;

        public string ActivityDepartment => this.Driver.GetElement(this.department).Text;

        public List<string> ActivityAttendees => this.Driver.GetElements(this.attendees).Select(x => x.Text).ToList();

        public ViewActivityPage SaveActivity()
        {
            this.Driver.Click(this.saveButton);
            return new ViewActivityPage(this.DriverContext);
        }

        public CreateActivityPage SelectActivityType(string activityType)
        {
            this.Driver.GetElement<Select>(this.type).SelectByText(activityType);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public CreateActivityPage SelectActivityStatus(string activityStatus)
        {
            this.Driver.GetElement<Select>(this.status).SelectByText(activityStatus);
            this.Driver.WaitForAngularToFinish();
            return this;
        }

        public Dictionary<string, string> GetPropertyAddress()
        {
            var propertyAddress = new Dictionary<string, string>
            {
                { "number", this.Driver.GetElement(this.propertyNumber).Text },
                { "name", this.Driver.GetElement(this.propertyName).Text },
                { "line2", this.Driver.GetElement(this.propertyLine2).Text },
                { "postCode", this.Driver.GetElement(this.propertyPostCode).Text },
                { "city", this.Driver.GetElement(this.propertyCity).Text },
                { "county", this.Driver.GetElement(this.propertyCounty).Text }
            };
            return propertyAddress;
        }

        public CreateActivityPage SelectSource(string sourceText)
        {
            this.Driver.GetElement<Select>(this.source).SelectByText(sourceText);
            return this;
        }

        public CreateActivityPage SetSourceDescription(string sourceDescriptionText)
        {
            this.Driver.SendKeys(this.sourceDescription, sourceDescriptionText);
            return this;
        }

        public CreateActivityPage SelectSellingReason(string reason)
        {
            this.Driver.GetElement<Select>(this.sellingReason).SelectByText(reason);
            return this;
        }

        public CreateActivityPage SetPitchingThreats(string threats)
        {
            this.Driver.SendKeys(this.pitchingThreats, threats);
            return this;
        }

        public CreateActivityPage SetKeyNumber(string key)
        {
            this.Driver.SendKeys(this.keyNumber, key);
            return this;
        }

        public CreateActivityPage SetAccessArangements(string arangements)
        {
            this.Driver.SendKeys(this.accessArrangements, arangements);
            return this;
        }

        public CreateActivityPage SetDate()
        {
            this.Driver.SendKeys(this.date, DateTime.Today.AddDays(1).ToString("dd-MM-yyyy"));
            return this;
        }

        public CreateActivityPage SetTime(string start, string end)
        {
            this.Driver.SendKeys(this.startTime, start);
            this.Driver.SendKeys(this.endTime, end);
            return this;
        }

        public CreateActivityPage SelectAtendee(string attendee)
        {
            this.Driver.GetElement(this.attendeeToSelect.Format(attendee)).Click();
            return this;
        }

        public CreateActivityPage SetInvitationText(string invitation)
        {
            this.Driver.SendKeys(this.invitationText, invitation);
            return this;
        }
    }

    internal class ActivityDetails
    {
        public string Vendor { get; set; }

        public string Status { get; set; }

        public string Type { get; set; }

        public string Negotiator { get; set; }

        public string CreationDate { get; set; }

        public string ActivityTitle { get; set; }

        public string Department { get; set; }

        public string Attendees { get; set; }

        public string Source { get; set; }

        public string SourceDescription { get; set; }

        public string SellingReason { get; set; }

        public string PitchingThreats { get; set; }

        public string KeyNumber { get; set; }

        public string AccessArangements { get; set; }
    }

    internal class ActivityAttendees
    {
        public string Attendees { get; set; }
    }
}
