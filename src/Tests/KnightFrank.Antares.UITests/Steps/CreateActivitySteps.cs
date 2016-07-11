namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class CreateActivitySteps
    {
        private readonly DriverContext driverContext;
        private readonly CreateActivityPage page;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;

        public CreateActivitySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new CreateActivityPage(this.driverContext);
            }
        }

        [When(@"User selects (.*) type on create activity page")]
        public void SelectActivityType(string type)
        {
            this.page.SelectActivityType(type);
        }

        [When(@"User selects (.*) status on create activity page")]
        public void SelectActivityStatus(string status)
        {
            this.page.SelectActivityStatus(status);
        }

        [When(@"User selects (.*) from source list on create activity page")]
        public void SelectSource(string source)
        {
            this.page.SelectSource(source);
        }

        [When(@"User fills in source description (.*) on create activity page")]
        public void AddSourceDescription(string description)
        {
            this.page.SetSourceDescription(description);
        }

        [When(@"User selects (.*) from selling reason list on create activity page")]
        public void SelectsSellingReason(string reason)
        {
            this.page.SelectSellingReason(reason);
        }

        [When(@"User fills in pitching threats (.*) on create activity page")]
        public void AddPitchingThreats(string pitchingThreats)
        {
            this.page.SetPitchingThreats(pitchingThreats);
        }

        [When(@"User fills in (.*) key number on create activity page")]
        public void FillInKeyNumber(string key)
        {
            this.page.SetKeyNumber(key);
        }

        [When(@"User fills in access arangements (.*) on create activity page")]
        public void FillInAccessArangements(string arangements)
        {
            this.page.SetAccessArangements(arangements);
        }

        [When(@"User sets appraisal meeting date as tomorrow date on create activity page")]
        public void SetDate()
        {
            this.page.SetDate();
        }

        [When(@"User sets start time at (.*) and end time at (.*) on create activity page")]
        public void SetTime(string startTime, string endTime)
        {
            this.page.SetTime(startTime, endTime);
        }

        [When(@"User selects (.*) from attendees on create activity page")]
        public void SelectAttendee(string attendee)
        {
            this.page.SelectAtendee(attendee);
        }

        [When(@"User fills in invitation text (.*) on create activity page")]
        public void AddInvitationText(string invitation)
        {
            this.page.SetInvitationText(invitation);
        }

        [When(@"User clicks save button on create activity page")]
        public void ClickSaveButtonOnActivityPanel()
        {
            this.page.SaveActivity();
        }

        [Then(@"Property details are set on create activity page")]
        public void CheckPropertyDetails(Table table)
        {
            var expectedAddress = table.CreateInstance<Address>();
            Dictionary<string, string> actualResult = this.page.GetPropertyAddress();

            Verify.That(this.driverContext, () => Assert.Equal(expectedAddress.PropertyNumber, actualResult["number"]),
                () => Assert.Equal(expectedAddress.PropertyName, actualResult["name"]),
                () => Assert.Equal(expectedAddress.Line2, actualResult["line2"]),
                () => Assert.Equal(expectedAddress.Postcode, actualResult["postCode"]),
                () => Assert.Equal(expectedAddress.City, actualResult["city"]),
                () => Assert.Equal(expectedAddress.County, actualResult["county"]));
        }

        [Then(@"Attendees are set on create activity page")]
        public void CheckAttendees(Table table)
        {
            List<string> expectedAttendees = table.CreateSet<ActivityAttendees>().Select(x => x.Attendees).ToList();
            List<string> actualAttendess = this.page.ActivityAttendees;
            expectedAttendees.ShouldBeEquivalentTo(actualAttendess);
        }

        [Then(@"Activity details are set on create activity page")]
        public void CheckActivityDetailsOnActivityPanel(Table table)
        {
            var details = table.CreateInstance<ActivityDetails>();

            Verify.That(this.driverContext,
                () => Assert.Equal(details.Vendor, this.page.ActivityVendor),
                () => Assert.Equal(details.Negotiator, this.page.ActivityNegotiator),
                () => Assert.Equal(details.ActivityTitle, this.page.ActivityTitle),
                () => Assert.Equal(details.Department, this.page.ActivityDepartment));
        }
    }
}
