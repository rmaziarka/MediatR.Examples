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

        [When(@"User selects activity status and type on create actvity page")]
        public void SelectStatusAndType(Table table)
        {
            var details = table.CreateInstance<ActivityDetails>();
            this.page.SelectActivityType(details.Type)
                .SelectActivityStatus(details.Status);
        }

        [When(@"User select (.*) from disposal type list on create activity page")]
        public void SelectDisposalType(string type)
        {
            this.page.SelectDisposalType(type);
        }

        [When(@"User fills in basic information on create activity page")]
        public void FillInBasicInformation(Table table)
        {
            var basicInformation = table.CreateInstance<ActivityDetails>();

            this.page.SelectSource(basicInformation.Source)
                .SetSourceDescription(basicInformation.SourceDescription)
                .SetPitchingThreats(basicInformation.PitchingThreats);
        }

        [When(@"User fills in additional information on create activity page")]
        public void AddAdditionalInformation(Table table)
        {
            var additionalInformation = table.CreateInstance<ActivityDetails>();

            this.page.SetKeyNumber(additionalInformation.KeyNumber)
                .SetAccessArangements(additionalInformation.AccessArrangements);
        }

        [When(@"User fills in attendees on create activity page")]
        public void AddAttendees(Table table)
        {
            var attendees = table.CreateInstance<ActivityDetails>();

            this.page.SelectAttendee(attendees.Attendees)
                .SetInvitationText(attendees.InvitationText);
        }

        [When(@"User fills in appraisal meeting information on create activity page")]
        public void SetDateAndTime(Table table)
        {
            var dateAndTime = table.CreateInstance<ActivityDetails>();

            this.page.SetTime(dateAndTime.StartTime, dateAndTime.EndTime).SetDate();
        }

        [When(@"User fills in valuation information for freehold sale activity on create activity page")]
        public void AddValutationinformation(Table table)
        {
            var valutationInformation = table.CreateInstance<ValuationInformation>();
            this.page
                .SetKfValuation(valutationInformation.KfValuation)
                .SetVendorValuation(valutationInformation.VendorValuation)
                .SetAgreedInitialMarketingPrice(valutationInformation.AgreedInitialMarketingPrice);
        }

        [When(@"User fills in other information on create activity page")]
        public void AddOtherInformation(Table table)
        {
            var otherInformation = table.CreateInstance<ActivityDetails>();
            this.page
                .SelectDecoration(otherInformation.Decoration)
                .SetOtherConditions(otherInformation.OtherConditions);
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
            //TODO change to list select
            this.page.SelectAttendee(attendee);
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

        [When(@"User selects (.*) from source list on create activity page")]
        public void SelectSource(string source)
        {
            this.page.SelectSource(source);
        }

        [When(@"User fills in price details on create activity page")]
        public void FillInPriceDetails(Table table)
        {
            var priceDetails = table.CreateInstance<Prices>();
            this.page.SelectPriceType(priceDetails.PriceType)
                .SetPrice(priceDetails.Price)
                .SelectMatchFlexibility(priceDetails.MatchFlexibility)
                .SetMatchFlexibilityValue(priceDetails.MatchFlexibility, priceDetails.MatchFlexibilityValue);
        }

        [When(@"User fills in rent details on create activity page")]
        public void FillInRentDetails(Table table)
        {
            var rentDetails = table.CreateInstance<LettingRent>();
            this.page.SelectRent(rentDetails.Rent)
                .SetAskingRentShortLet(rentDetails.RentShortLet)
                .SelectShortLetMatchFlexibility(rentDetails.RentShortLetMatchFlexibility)
                .SetShortLetMatchFlexibilityRentValue(rentDetails.RentShortLetMatchFlexibilityRentValue)
                .SetAskingRentLongLet(rentDetails.RentLongLet)
                .SelectLongLetMatchFlexibility(rentDetails.RentLongLetMatchFlexibility)
                .SetLongLetMatchFlexibilityRentPercentage(rentDetails.RentLongLetMatchFlexibilityRentPercentage);
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
            List<string> expectedAttendees = table.CreateSet<ActivityDetails>().Select(x => x.Attendees).ToList();
            List<string> actualAttendess = this.page.ActivityAttendees;
            expectedAttendees.ShouldBeEquivalentTo(actualAttendess);
        }

        [Then(@"Sales activity details are set on create activity page")]
        public void CheckSalesActivityDetailsOnActivityPanel(Table table)
        {
            var details = table.CreateInstance<ActivityDetails>();

            Verify.That(this.driverContext,
                () => Assert.Equal(details.Vendor, this.page.ActivityVendor),
                () => Assert.Equal(details.Negotiator, this.page.ActivityNegotiator),
                () => Assert.Equal(details.ActivityTitle, this.page.ActivityTitle),
                () => Assert.Equal(details.Department, this.page.ActivityDepartment));
        }

        [Then(@"Letting activity details are set on create activity page")]
        public void CheckLettingActivityDetailsOnActivityPanel(Table table)
        {
            var details = table.CreateInstance<ActivityDetails>();

            Verify.That(this.driverContext,
                () => Assert.Equal(details.Landlord, this.page.ActivityLandlord),
                () => Assert.Equal(details.Negotiator, this.page.ActivityNegotiator),
                () => Assert.Equal(details.ActivityTitle, this.page.ActivityTitle),
                () => Assert.Equal(details.Department, this.page.ActivityDepartment));
        }
    }
}
