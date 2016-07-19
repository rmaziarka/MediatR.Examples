﻿namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.UITests.Pages;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ViewActivitySteps
    {
        private const string Format = "dd-MM-yyyy";
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;
        private ViewActivityPage page;

        public ViewActivitySteps(ScenarioContext scenarioContext)
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

        [When(@"User navigates to view activity page with id")]
        public void OpenViewActivityPageWithId()
        {
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            this.page = new ViewActivityPage(this.driverContext).OpenViewActivityPageWithId(activityId.ToString());
        }

        [When(@"User clicks property details on view activity page")]
        public void OpenPreviewPropertyPage()
        {
            this.page.ClickPropertyCard().WaitForSidePanelToShow();
        }

        [When(@"User clicks view property link from property on view activity page")]
        public void OpenViewPropertyPage()
        {
            this.page.PropertyPreview.OpenViewPropertyPage();
        }

        [When(@"User clicks edit button on view activity page")]
        public void EditActivity()
        {
            this.page.EditActivity();
        }

        [When(@"User clicks (.*) viewings details link on overview tab on view activity page")]
        public void OpenViewingsDetails(int position)
        {
            this.page.OpenViewingDetails(position).WaitForSidePanelToShow();
        }

        [When(@"User clicks requirement details from viewing on view activity page")]
        public void ClickViewActivity()
        {
            this.page.ViewingDetails.OpenActions().ClickDetailsLink();
        }

        [When(@"User clicks (.*) offer details on view activity page")]
        public void OpenOffersDetails(int position)
        {
            this.page.OpenOfferDetails(position)
                .WaitForSidePanelToShow();
            this.page.OfferPreview.WaitForDetailsToLoad();
        }

        [When(@"User edits lead negotiator next call to (.*) days from current day on view activity page")]
        public void UpdateLeadNegotiatorNextCall(int days)
        {
            this.page.EditLeadNegotiatorNextCall(days);
        }

        [When(@"User edits secondary negotiator next call on view activity page")]
        public void UpdateSecondaryNegotiatorsNextCall(Table table)
        {
            IEnumerable<Negotiator> secondaryNegotiatorsNextCall = table.CreateSet<Negotiator>();
            foreach (Negotiator negotiator in secondaryNegotiatorsNextCall)
            {
                this.page.EditSecondaryNegotiatorNextCall(negotiator.Name, int.Parse(negotiator.NextCall));
            }
        }

        [When(@"User switches to details tab on view activity page")]
        public void SwitchToDetailsTab()
        {
            this.page.OpenDetailsTab();
        }

        [When(@"User switches to overview tab on view activity page")]
        public void SwitchToOverviewTab()
        {
            this.page.OpenOverviewTab();
        }

        [When(@"User switches to attachments tab on view activity page")]
        public void SwitchToAttachmentsTab()
        {
            this.page.OpenAttachmentsTab();
        }

        [When(@"User switches to marketing tab on view activity page")]
        public void SwitchToMarketingTab()
        {
            this.page.OpenMarketingTab();
        }

        [Then(@"Address details on view activity page are following")]
        public void CheckViewActivityAddressDetails(Table table)
        {
            foreach (string field in table.Rows.SelectMany(row => row.Values))
            {
                Assert.True(field.Equals(string.Empty)
                    ? this.page.IsAddressDetailsNotVisible(field)
                    : this.page.IsAddressDetailsVisible(field));
            }
        }

        [Then(@"Activity details should be displayed on view activity page")]
        public void CheckActivityOverviewDetails(Table table)
        {
            //TODO improve chekcing details
            var expectedDetails = table.CreateInstance<ActivityDetails>();
            Dictionary<string, string> actualDetails = this.page.GetActivityDetails();
            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.ActivityTitle, actualDetails["title"]),
                () => Assert.Equal(expectedDetails.Status, actualDetails["status"]),
                () => Assert.Equal(expectedDetails.Type, actualDetails["type"]));
        }

        [Then(@"Activity details on view activty page are following")]
        public void CheckActivityDetails(Table table)
        {
            //TODO improve chekcing details
            var details = table.CreateInstance<EditActivityDetails>();
            Verify.That(this.driverContext,
                () => Assert.Equal(details.ActivityStatus, this.page.Status));
        }

        [Then(@"View activity page should be displayed")]
        public void CheckIfViewActivityPresent()
        {
            Assert.True(this.page.IsViewActivityFormPresent());
        }

        [Then(@"Viewing details on (.*) position on overview tab on view activity page are same as the following")]
        public void CheckViewing(int position, Table table)
        {
            var expectedDetails = table.CreateInstance<ViewingData>();
            List<string> actualDetails = this.page.GetViewingDetails(position);

            Verify.That(this.driverContext, () => Assert.Equal(this.page.ViewingsNumber, 1),
                () => Assert.Equal(expectedDetails.Name, actualDetails[0]),
                () => Assert.Equal(expectedDetails.Date + ",", actualDetails[1]),
                () => Assert.Equal(expectedDetails.Time, actualDetails[2]),
                () => Assert.Equal(expectedDetails.Negotiator, actualDetails[3]));
        }

        [Then(@"Viewing details on view activity page are same as the following")]
        public void CheckViewingInDetailsPanel(Table table)
        {
            var expectedDetails = table.CreateInstance<ViewingDetails>();
            List<string> attendees = expectedDetails.Attendees.Split(';').ToList();

            Verify.That(this.driverContext, 
                () => Assert.Equal(expectedDetails.Name, this.page.ViewingDetails.Details),
                () => Assert.Equal(expectedDetails.Date + ", " + expectedDetails.StartTime + " - " + expectedDetails.EndTime, this.page.ViewingDetails.Date),
                () => Assert.Equal(expectedDetails.Negotiator, this.page.ViewingDetails.Negotiator),
                () => Assert.Equal(attendees, this.page.ViewingDetails.Attendees),
                () => Assert.Equal(expectedDetails.InvitationText, this.page.ViewingDetails.InvitationText),
                () => Assert.Equal(expectedDetails.PostViewingComment, this.page.ViewingDetails.PostViewingComment));
        }

        [Then(@"(.*) is set as lead negotiator on view activity page")]
        public void CheckLeadNegotiator(string expectedLead)
        {
            Assert.Equal(expectedLead, this.page.LeadNegotiator);
        }

        [Then(@"Secondary negotiators are set on view activity page")]
        public void CheckSecondaryUsers(Table table)
        {
            List<Negotiator> expectedSecondary = table.CreateSet<Negotiator>().ToList();
            foreach (Negotiator secondaryNegotiator in expectedSecondary)
            {
                secondaryNegotiator.NextCall = secondaryNegotiator.NextCall != "-"
                    ? DateTime.UtcNow.AddDays(int.Parse(secondaryNegotiator.NextCall)).ToString(Format)
                    : "-";
            }

            List<Negotiator> actual = this.page.GetSecondaryNegotiatorsData();

            expectedSecondary.Should().Equal(actual, (n1, n2) => n1.Name.Equals(n2.Name) && n1.NextCall.Equals(n2.NextCall));
        }

        [Then(@"Offer should be displayed on overview tab on view activity page")]
        public void CheckIfOfferDisplayed()
        {
            Assert.Equal(1, this.page.OffersNumber);
            Assert.Equal(1, int.Parse(this.page.OffersCounter));
        }

        [Then(@"Viewing should be displayed on overview tab on view activity page")]
        public void CheckIViewingDisplayed()
        {
            Assert.Equal(1, this.page.ViewingsNumber);
            Assert.Equal(1, int.Parse(this.page.ViewingsCounter));
        }

        [Then(@"Letting offer details on (.*) position on overview tab on view activity page are same as the following")]
        public void CheckLettingOffer(int position, Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();
            List<string> actualDetails = this.page.GetOfferDetails(position);

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Details, actualDetails[0]),
                () => Assert.Equal(int.Parse(expectedDetails.OfferPerWeek).ToString("N0") + " GBP / week", actualDetails[1]),
                () => Assert.Equal(expectedDetails.Negotiator, actualDetails[2]),
                () => Assert.Equal(this.scenarioContext.Get<OfferData>("Offer").ExchangeDate, actualDetails[3]),
                () => Assert.Equal(expectedDetails.Status, actualDetails[4]));
        }

        [Then(@"Sale offer details on (.*) position on overview tab on view activity page are same as the following")]
        public void CheckSalesOffer(int position, Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();
            List<string> actualDetails = this.page.GetOfferDetails(position);

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Details, actualDetails[0]),
                () => Assert.Equal(int.Parse(expectedDetails.Offer).ToString("N0") + " GBP", actualDetails[1]),
                () => Assert.Equal(expectedDetails.Negotiator, actualDetails[2]),
                () => Assert.Equal(this.scenarioContext.Get<OfferData>("Offer").ExchangeDate, actualDetails[3]),
                () => Assert.Equal(expectedDetails.Status, actualDetails[4]));
        }

        [Then(@"Letting offer details on view activity page are same as the following")]
        public void CheckLettingOfferInDetailsPanel(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();
            var offer = this.scenarioContext.Get<OfferData>("Offer");

            expectedDetails.OfferDate = offer.OfferDate;
            expectedDetails.ExchangeDate = offer.ExchangeDate;
            expectedDetails.CompletionDate = offer.CompletionDate;

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Details, this.page.OfferPreview.GetRequirementDetails()),
                () => Assert.Equal(expectedDetails.Status, this.page.OfferPreview.Status),
                () => Assert.Equal(int.Parse(expectedDetails.OfferPerWeek).ToString("N0") + " GBP / week", this.page.OfferPreview.OfferPerWeek),
                () => Assert.Equal(expectedDetails.OfferDate, this.page.OfferPreview.Date),
                () => Assert.Equal(expectedDetails.SpecialConditions, this.page.OfferPreview.SpecialConditions),
                () => Assert.Equal(expectedDetails.Negotiator, this.page.OfferPreview.Negotiator),
                () => Assert.Equal(expectedDetails.ExchangeDate, this.page.OfferPreview.ProposedexchangeDate),
                () => Assert.Equal(expectedDetails.CompletionDate, this.page.OfferPreview.ProposedCompletionDate));
        }

        [Then(@"Sale offer details on view activity page are same as the following")]
        public void CheckSaleOfferInDetailsPanel(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();
            var offer = this.scenarioContext.Get<OfferData>("Offer");

            expectedDetails.OfferDate = offer.OfferDate;
            expectedDetails.ExchangeDate = offer.ExchangeDate;
            expectedDetails.CompletionDate = offer.CompletionDate;

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Details, this.page.OfferPreview.GetRequirementDetails()),
                () => Assert.Equal(expectedDetails.Status, this.page.OfferPreview.Status),
                () => Assert.Equal(int.Parse(expectedDetails.Offer).ToString("N0") + " GBP", this.page.OfferPreview.Offer),
                () => Assert.Equal(expectedDetails.OfferDate, this.page.OfferPreview.Date),
                () => Assert.Equal(expectedDetails.SpecialConditions, this.page.OfferPreview.SpecialConditions),
                () => Assert.Equal(expectedDetails.Negotiator, this.page.OfferPreview.Negotiator),
                () => Assert.Equal(expectedDetails.ExchangeDate, this.page.OfferPreview.ProposedexchangeDate),
                () => Assert.Equal(expectedDetails.CompletionDate, this.page.OfferPreview.ProposedCompletionDate));
        }

        [Then(@"Lead negotiator next call is set to (.*) days from current day on view activity page")]
        public void GetDefaultNextCallDateForLeadNegotiator(int day)
        {
            Assert.Equal(DateTime.UtcNow.AddDays(day).ToString(Format), this.page.LeadNegotiatorNextCall);
        }

        [Then(@"Departments should be displayed on view activity page")]
        public void CheckDepartment(Table table)
        {
            List<Department> expectedDepartments = table.CreateSet<Department>().ToList();
            List<ViewActivityPage.Department> actualDepartments = this.page.Departments;
            expectedDepartments.Should().Equal(actualDepartments, (d1, d2) => d1.Name.Equals(d2.Name));
        }

        [Then(@"Property details should be displayed in overview tab on view activity page")]
        public void CheckPropertyDetailsInOverviewTab(Table table)
        {
            var expectedAddress = table.CreateInstance<Address>();
            Dictionary<string, string> actualResult = this.page.GetPropertyAddressOnOverviewTab();
            Verify.That(this.driverContext,
                () => Assert.Equal(expectedAddress.PropertyNumber, actualResult["number"]),
                () => Assert.Equal(expectedAddress.PropertyName, actualResult["name"]),
                () => Assert.Equal(expectedAddress.Line2, actualResult["line2"]),
                () => Assert.Equal(expectedAddress.Postcode, actualResult["postCode"]),
                () => Assert.Equal(expectedAddress.City, actualResult["city"]),
                () => Assert.Equal(expectedAddress.County, actualResult["county"]));
        }

        [Then(@"Sales activity details should be displayed in overview tab on view activity page")]
        public void CheckActivityDetailsforSaleTypeInOverviewTab(Table table)
        {
            var expectedDetails = table.CreateInstance<ActivityDetails>();
            Dictionary<string, string> actualDetails = this.page.GetActivityDetailsForSaleTypeOnOverviewTab();
            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Vendor, actualDetails["vendor"]),
                () => Assert.Equal(expectedDetails.Negotiator, actualDetails["negotiator"]),
                () => Assert.Equal(expectedDetails.Attendees, actualDetails["attendee"]));
        }

        [Then(@"Letting activity details should be displayed in overview tab on view activity page")]
        public void CheckLettingActivityDetailsInOverviewTab(Table table)
        {
            var expectedDetails = table.CreateInstance<ActivityDetails>();
            Dictionary<string, string> actualDetails = this.page.GetActivityDetailsForLettingTypeOnOverviewTab();
            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Landlord, actualDetails["landlord"]),
                () => Assert.Equal(expectedDetails.Negotiator, actualDetails["negotiator"]),
                () => Assert.Equal(expectedDetails.Attendees, actualDetails["attendee"]));
        }


        [Then(@"Appraisal meeting date is set to tomorrow date with start time (.*) in overview tab on view activity page")]
        public void CheckDateAndTime(string time)
        {
            string date = DateTime.Today.AddDays(1).ToString("dd-MM-yyyy");
            Dictionary<string, string> actualDateTime = this.page.GetAppraisalDateAndTime();
            Verify.That(this.driverContext,
                () => Assert.Equal(date, actualDateTime["date"]),
                () => Assert.Equal(time, actualDateTime["time"]));
        }

        [Then(@"Sales activity details should be displayed in details tab on view activity page")]
        public void CheckSalesActivityDetailsInDetailsTab(Table table)
        {
            var expectedDetails = table.CreateInstance<ActivityDetails>();
            Dictionary<string, string> actualDetails = this.page.GetSalesActivityDetailsOnDetailsTab();
            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Vendor, actualDetails["vendor"]),
                () => Assert.Equal(expectedDetails.Negotiator, actualDetails["negotiator"]),
                () => Assert.Equal(expectedDetails.Department, actualDetails["department"]),
                () => Assert.Equal(expectedDetails.Source, actualDetails["source"]),
                () => Assert.Equal(expectedDetails.SourceDescription, actualDetails["sourceDescription"]),
                () => Assert.Equal(expectedDetails.SellingReason, actualDetails["sellingReason"]),
                () => Assert.Equal(expectedDetails.PitchingThreats, actualDetails["pitchingThreats"]),
                () => Assert.Equal(expectedDetails.KeyNumber, actualDetails["keyNumber"]),
                () => Assert.Equal(expectedDetails.AccessArrangements, actualDetails["accessArangements"]));
        }

        [Then(@"Letting activity details should be displayed in details tab on view activity page")]
        public void CheckLettingActivityDetailsInDetailsTab(Table table)
        {
            var expectedDetails = table.CreateInstance<ActivityDetails>();
            Dictionary<string, string> actualDetails = this.page.GetLettingActivityDetailsOnDetailsTab();
            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Landlord, actualDetails["landlord"]),
                () => Assert.Equal(expectedDetails.Negotiator, actualDetails["negotiator"]),
                () => Assert.Equal(expectedDetails.Department, actualDetails["department"]),
                () => Assert.Equal(expectedDetails.Source, actualDetails["source"]),
                () => Assert.Equal(expectedDetails.SourceDescription, actualDetails["sourceDescription"]),
                () => Assert.Equal(expectedDetails.PitchingThreats, actualDetails["pitchingThreats"]),
                () => Assert.Equal(expectedDetails.KeyNumber, actualDetails["keyNumber"]),
                () => Assert.Equal(expectedDetails.AccessArrangements, actualDetails["accessArangements"]));
        }


        [Then(@"Other activity details are displayed in details tab on view activity page")]
        public void CheckOtherActivityDetailsInDetailsTab(Table table)
        {
            var expectedDetails = table.CreateInstance<ActivityDetails>();
            Dictionary<string, string> actualDetails = this.page.GetOtherActivityDetailsOnDetailsTab();
            Verify.That(this.driverContext,
                 () => Assert.Equal(expectedDetails.Decoration, actualDetails["decoration"]),
                 () => Assert.Equal(expectedDetails.OtherConditions, actualDetails["otherConditions"]));
        }


        [Then(@"Property details should be displayed in details tab on view activity page")]
        public void CheckPropertyDetailsOnDetailsTab(Table table)
        {
            var expectedAddress = table.CreateInstance<Address>();
            Dictionary<string, string> actualResult = this.page.GetPropertyAddressOnOverviewTab();
            Verify.That(this.driverContext,
                () => Assert.Equal(expectedAddress.PropertyNumber, actualResult["number"]),
                () => Assert.Equal(expectedAddress.PropertyName, actualResult["name"]),
                () => Assert.Equal(expectedAddress.Line2, actualResult["line2"]),
                () => Assert.Equal(expectedAddress.Postcode, actualResult["postCode"]),
                () => Assert.Equal(expectedAddress.City, actualResult["city"]),
                () => Assert.Equal(expectedAddress.County, actualResult["county"]));
        }

        [Then(@"Valuation information details are displayed in details tab on view activity page")]
        public void CheckPropertyDetailsonDetailsTabForFreeholdSaleActivity(Table table)
        {
            var expectedDetails = table.CreateInstance<ValuationInformation>();
            Dictionary<string, string> actualDetails = this.page.GetActivityDetailsOnDetailsTabForFreeholdSale();
            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.DisposalType, actualDetails["disposalType"]),
                () => Assert.Equal(int.Parse(expectedDetails.KfValuation).ToString("N0") + " GBP", actualDetails["kfValuation"]),
                () => Assert.Equal(int.Parse(expectedDetails.VendorValuation).ToString("N0") + " GBP", actualDetails["vendorValuation"]),
                () => Assert.Equal(int.Parse(expectedDetails.AgreedInitialMarketingPrice).ToString("N0") + " GBP", actualDetails["agreedInitialMarketingPrice"]));
        }

        [Then(@"Price details for match flexibility wtih minimum price are following on details tab on view activity page")]
        public void CheckPriceDetailsForMinimumPriceOnDetailsTab(Table table)
        {
            var expectedDetails = table.CreateInstance<Prices>();
            Dictionary<string, string> actualDetails = this.page.GetActivityPriceDetailsFormMinimumPriceOnDetailsTab();
            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.PriceType, actualDetails["priceType"]),
                () => Assert.Equal(int.Parse(expectedDetails.Price).ToString("N0") + " GBP", actualDetails["price"]),
                () => Assert.Equal(int.Parse(expectedDetails.MatchFlexibilityValue).ToString("N0"), actualDetails["matchFlexibilityValue"]));
        }

        [Then(@"Price details for match flexibility with percentage are following on details tab on view activity page")]
        public void CheckPriceDetailsForPercentageOnDetailsTab(Table table)
        {
            var expectedDetails = table.CreateInstance<Prices>();
            Dictionary<string, string> actualDetails = this.page.GetActivityPriceDetailsForPercentageOnDetailsTab();
            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.PriceType, actualDetails["priceType"]),
                () => Assert.Equal(int.Parse(expectedDetails.Price).ToString("N0") + " GBP", actualDetails["price"]),
                () => Assert.Equal(expectedDetails.MatchFlexibilityValue + " %", actualDetails["matchFlexibilityValue"]));
        }

        [Then(@"Rent details are following on details tab on view activity page")]
        public void CheckRentDetailsOnDetailsTab(Table table)
        {
            var expectedDetails = table.CreateInstance<LettingRent>();
            Dictionary<string, string> actualDetails = this.page.GetLettingActivityRentDetailsOnDetailsTab();
            Verify.That(this.driverContext,
                () => Assert.Equal(int.Parse(expectedDetails.RentShortLetMonth).ToString("N0") + " GBP / month", actualDetails["rentShortLetMonth"]),
                () => Assert.Equal(int.Parse(expectedDetails.RentShortLetWeek).ToString("N0") + " GBP / week", actualDetails["rentShortLetWeek"]),
                () => Assert.Equal(int.Parse(expectedDetails.RentShortMatchFlexibilityMonth).ToString("N0"), actualDetails["rentShortMatchFlexibilityMonth"]),
                () => Assert.Equal(int.Parse(expectedDetails.RentShortMatchFlexibilityWeek).ToString("N0"), actualDetails["rentShortMatchFlexibilityWeek"]),
                () => Assert.Equal(int.Parse(expectedDetails.RentLongLetMonth).ToString("N0") + " GBP / month", actualDetails["rentLongLetMonth"]),
                () => Assert.Equal(int.Parse(expectedDetails.RentLongtLetWeek).ToString("N0") + " GBP / week", actualDetails["rentLongtLetWeek"]),
                () =>Assert.Equal(expectedDetails.RentLongMatchFlexibilityPercentage + " %", actualDetails["rentLongMatchFlexibilityPercentage"]));
        }

        [Then(@"Edited rent details are following on details tab on view activity page")]
        public void CheckEditedRentDetailsOnDetailsTab(Table table)
        {
            var expectedDetails = table.CreateInstance<LettingRent>();
            Dictionary<string, string> actualDetails = this.page.GetEditedLettingActivityRentDetailsOnDetailsTab();
            Verify.That(this.driverContext,
                () => Assert.Equal(int.Parse(expectedDetails.RentShortLetMonth).ToString("N0") + " GBP / month", actualDetails["rentShortLetMonth"]),
                () => Assert.Equal(int.Parse(expectedDetails.RentShortLetWeek).ToString("N0") + " GBP / week", actualDetails["rentShortLetWeek"]),
                () => Assert.Equal(expectedDetails.RentShortMatchFlexibilityPercentage + " %", actualDetails["rentShortMatchFlexibilityPercentage"]),
                () => Assert.Equal(int.Parse(expectedDetails.RentLongLetMonth).ToString("N0") + " GBP / month", actualDetails["rentLongLetMonth"]),
                () => Assert.Equal(int.Parse(expectedDetails.RentLongtLetWeek).ToString("N0") + " GBP / week", actualDetails["rentLongtLetWeek"]),
                () => Assert.Equal(int.Parse(expectedDetails.RentLongMatchFlexibilityMonth).ToString("N0"), actualDetails["rentLongMatchFlexibilityMonth"]),
                () => Assert.Equal(int.Parse(expectedDetails.RentLongMatchFlexibilityWeek).ToString("N0"), actualDetails["rentLongMatchFlexibilityWeek"]));
        }

        [Then(@"Edited price details for match flexibility with percentage are following on details tab on view activity page")]
        public void CheckdPriceDetailsForPercentageOnDetailsTab(Table table)
        {
            var expectedDetails = table.CreateInstance<Prices>();
            Dictionary<string, string> actualDetails = this.page.GetActivityPriceDetailsFormMinimumPriceOnDetailsTab();
            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.PriceType, actualDetails["priceType"]),
                () => Assert.Equal(int.Parse(expectedDetails.Price).ToString("N0") + " GBP", actualDetails["price"]),
                () => Assert.Equal(int.Parse(expectedDetails.MatchFlexibilityValue).ToString("N0"), actualDetails["matchFlexibilityValue"]));
        }

    }
}
