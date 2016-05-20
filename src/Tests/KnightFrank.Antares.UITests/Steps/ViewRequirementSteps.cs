namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.UITests.Pages;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ViewRequirementSteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;
        private const string Format = "dd-MM-yyyy";

        public ViewRequirementSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [When(@"User navigates to view requirement page with id")]
        public void OpenViewRequirementPageWithId()
        {
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            ViewRequirementPage page =
                new ViewRequirementPage(this.driverContext).OpenViewRequirementPageWithId(requirementId.ToString());
            this.scenarioContext.Set(page, "ViewRequirementPage");
        }

        [When(@"User clicks notes button on view requirement page")]
        public void OpenNotes()
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").OpenNotes().WaitForSidePanelToShow();
        }

        [When(@"User adds note on view requirement page")]
        public void InsertNotesText(Table table)
        {
            var details = table.CreateInstance<RequirementNote>();
            var page = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage");
            page.Notes.SetNoteText(details.Description).SaveNote();
        }

        [When(@"User clicks add viewings button on view requirement page")]
        public void ClickAddViewings()
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").AddViewings().WaitForSidePanelToShow();
        }

        [When(@"User selects activity on view requirement page")]
        public void SelectActivity(Table table)
        {
            var details = table.CreateInstance<Address>();
            var page = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage");

            string activity = details.PropertyName + ", " + details.PropertyNumber + " " + details.Line2;
            page.ActivityList.WaitForDetailsToLoad().SelectActivity(activity);
        }

        [When(@"User fills in viewing details on view requirement page")]
        public void FillInViewingDetails(Table table)
        {
            var details = table.CreateInstance<ViewingDetails>();
            var page = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage");

            page.Viewing
                .SetDate(details.Date)
                .SetStartTime(details.StartTime)
                .SetEndTime(details.EndTime)
                .SetInvitation(details.InvitationText)
                .SetPostViewingComment(details.PostViewingComment);
        }

        [When(@"User selects attendees for viewing on view requirement page")]
        [When(@"User unselects attendees for viewing on view requirement page")]
        public void SelectAttendeesForViewing(Table table)
        {
            var page = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage");
            List<string> attendees = table.Rows.Select(r => r[0]).ToList();

            page.Viewing.SelectAttendees(attendees);
        }

        [When(@"User clicks save activity button on view requirement page")]
        public void SaveViewing()
        {
            var page = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage");
            page.Viewing.SaveViewing();
            page.WaitForSidePanelToHide();
        }

        [When(@"User clicks (.*) viewings details on view requirement page")]
        public void OpenViewingsDetails(int position)
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage")
                .OpenViewingDetails(position)
                .WaitForSidePanelToShow();
        }
        
        [When(@"User clicks (.*) offer details on view requirement page")]
        public void OpenOffersDetails(int position)
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage")
                .OpenOfferDetails(position)
                .WaitForSidePanelToShow();
        }

        [When(@"User clicks edit activity button on view requirement page")]
        public void ClickEditActivity()
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").ViewingDetails.EditViewing();
        }

        [When(@"User clicks view activity from viewing on view requirement page")]
        public void ClickViewActivityViewing()
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").ViewingDetails.ClickViewLink();
        }

        [When(@"User clicks view activity from offer on view requirement page")]
        public void ClickViewActivityOffer()
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").OfferPreview.ClickViewLink();
        }

        [When(@"User clicks make an offer button for (.*) activity on view requirement page")]
        public void MakeAnOffer(int position)
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage")
                .OpenViewingActions(position)
                .CreateOffer(1)
                .WaitForSidePanelToShow();
        }

        [When(@"User fills in offer details on view requirement page")]
        public void FIllOfferDetails(Table table)
        {
            var details = table.CreateInstance<OfferData>();
            var page = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage");

            details.OfferDate = DateTime.UtcNow.ToString(Format);
            details.ExchangeDate = DateTime.UtcNow.AddDays(1).ToString(Format);
            details.CompletionDate = DateTime.UtcNow.AddDays(2).ToString(Format);

            page.Offer.SelectStatus(details.Status)
                .SetOffer(details.Offer)
                .SetOfferDate(details.OfferDate)
                .SetSpecialConditions(details.SpecialConditions)
                .SetProposedExchangeDate(details.ExchangeDate)
                .SetProposedCompletionDate(details.CompletionDate);
        }

        [When(@"User clicks save offer button on view requirement page")]
        public void SaveOffer()
        {
            var page = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage");
            page.Offer.SaveOffer();
            page.WaitForSidePanelToHide();
        }

        [Then(@"Requirement location details on view requirement page are same as the following")]
        public void CheckResidentialSalesRequirementLocationDetails(Table table)
        {
            Dictionary<string, string> details =
                this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage")
                    .GetLocationRequirements();
            var expectedDetails = table.CreateInstance<Address>();

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Line2, details["Street name"]),
                () => Assert.Equal(expectedDetails.Postcode, details["Postcode"]),
                () => Assert.Equal(expectedDetails.City, details["City"]));
        }

        [Then(@"Requirement property details on view requirement page are same as the following")]
        public void CheckResidentialSalesRequirementPropertyDetails(Table table)
        {
            Dictionary<string, string> details =
                this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage")
                    .GetPropertyRequirements();
            var expectedDetails = table.CreateInstance<Requirement>();

            Verify.That(this.driverContext,
                () => Assert.True(details["Price"].Contains(expectedDetails.MinPrice + " - " + expectedDetails.MaxPrice + " GBP"), "Prices are different"),
                () => Assert.True(details["Bedrooms"].Contains(expectedDetails.MinBedrooms + " - " + expectedDetails.MaxBedrooms), "Number of bedrooms is different"),
                () => Assert.True(details["Reception rooms"].Contains(expectedDetails.MinReceptionRooms + " - " + expectedDetails.MaxReceptionRooms), "Number of reception rooms is different"),
                () => Assert.True(details["Bathrooms"].Contains(expectedDetails.MinBathrooms + " - " + expectedDetails.MaxBathrooms), "Number of bathrooms is different"),
                () => Assert.True(details["Parking spaces"].Contains(expectedDetails.MinParkingSpaces + " - " + expectedDetails.MaxParkingSpaces), "Number of parking spaces is different"),
                () => Assert.True(details["Area"].Contains(expectedDetails.MinArea + " - " + expectedDetails.MaxArea + " sq ft"), "Areas are different"),
                () => Assert.True(details["Land area"].Contains(expectedDetails.MinLandArea + " - " + expectedDetails.MaxLandArea + " sq ft"), "Land areas are different"),
                () => Assert.Equal(expectedDetails.Description, details["Requirement description"]));
        }

        [Then(@"Requirement applicants on view requirement page are same as the following")]
        public void CheckResidentialSalesRequirementApplicants(Table table)
        {
            List<string> applicants =
                this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").Applicants;
            List<string> expectedApplicants =
                table.CreateSet<Contact>().Select(contact => contact.FirstName + " " + contact.Surname).ToList();

            Assert.Equal(expectedApplicants, applicants);
        }

        [Then(@"Note is displayed in recent notes area on view requirement page")]
        public void CheckIfNoteAdded()
        {
            Assert.Equal(1, this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").Notes.GetNumberOfNotes());
        }

        [Then(@"Notes number increased on view requirement page")]
        public void CheckIfNotesNumberIncreased()
        {
            string notesNumber = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").CheckNotesNumber();
            Assert.Equal("(1)", notesNumber);
        }

        [Then(@"Viewing details on (.*) position on view requirement page are same as the following")]
        public void CheckViewing(int position, Table table)
        {
            var page = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage");
            var expectedDetails = table.CreateInstance<ViewingData>();
            List<string> actualDetails = page.GetViewingDetails(position);

            Verify.That(this.driverContext, () => Assert.Equal(page.ViewingsNumber, 1),
                () => Assert.Equal(expectedDetails.Date, actualDetails[0]),
                () => Assert.Equal(expectedDetails.Name, actualDetails[1]),
                () => Assert.Equal(expectedDetails.Time, actualDetails[2]));
        }

        [Then(@"Offer details on (.*) position on view requirement page are same as the following")]
        public void CheckOffer(int position, Table table)
        {
            var page = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage");
            var expectedDetails = table.CreateInstance<OfferData>();
            List<string> actualDetails = page.GetOfferDetails(position);
            string status = page.GetOfferStatus(position);

            Verify.That(this.driverContext, 
                () => Assert.Equal(expectedDetails.Activity, actualDetails[0]),
                () => Assert.Equal(expectedDetails.Offer, actualDetails[1]),
                () => Assert.Equal(expectedDetails.Status, status));
        }

        [Then(@"Viewing details on view requirement page are same as the following")]
        public void CheckViewingInDetailsPanel(Table table)
        {
            var page = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage");
            var expectedDetails = table.CreateInstance<ViewingDetails>();

            List<string> attendees = expectedDetails.Attendees.Split(';').ToList();

            Verify.That(this.driverContext, 
                () => Assert.Equal(expectedDetails.Name, page.ViewingDetails.Details),
                () => Assert.Equal(expectedDetails.Date + ", " + expectedDetails.StartTime + " - " + expectedDetails.EndTime, page.ViewingDetails.Date),
                () => Assert.Equal(expectedDetails.Negotiator, page.ViewingDetails.Negotiator),
                () => Assert.Equal(attendees, page.ViewingDetails.Attendees),
                () => Assert.Equal(expectedDetails.InvitationText, page.ViewingDetails.InvitationText),
                () => Assert.Equal(expectedDetails.PostViewingComment, page.ViewingDetails.PostViewingComment));
        }

        [Then(@"Offer details on view requirement page are same as the following")]
        public void CheckOfferInDetailsPanel(Table table)
        {
            var page = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage");
            var expectedDetails = table.CreateInstance<OfferData>();
            expectedDetails.OfferDate = DateTime.UtcNow.ToString(Format);
            expectedDetails.ExchangeDate = DateTime.UtcNow.AddDays(1).ToString(Format);
            expectedDetails.CompletionDate = DateTime.UtcNow.AddDays(2).ToString(Format);

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Activity, page.OfferPreview.Details),
                () => Assert.Equal(expectedDetails.Status, page.OfferPreview.Status),
                () => Assert.Equal(expectedDetails.Offer, page.OfferPreview.Offer),
                () => Assert.Equal(expectedDetails.OfferDate, page.OfferPreview.Date),
                () => Assert.Equal(expectedDetails.SpecialConditions, page.OfferPreview.SpecialConditions),
                () => Assert.Equal(expectedDetails.Negotiator, page.OfferPreview.Negotiator),
                () => Assert.Equal(expectedDetails.ExchangeDate, page.OfferPreview.ProposedexchangeDate),
                () => Assert.Equal(expectedDetails.CompletionDate, page.OfferPreview.ProposedCompletionDate));
        }

        [Then(@"View requirement page is displayed")]
        public void CheckIfViewRequirementPresent()
        {
            var page = new ViewRequirementPage(this.driverContext);
            Assert.True(page.IsViewRequirementFormPresent());
            this.scenarioContext.Set(page, "ViewActivityPage");
        }

        [Then(@"Activity details on view requirement page are same as the following")]
        public void CheckOfferActivity(Table table)
        {
            var details = table.CreateInstance<OfferData>();
            Assert.Equal(details.Activity, this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").Offer.Details);
        }

        [Then(@"New offer should be created and displayed on view requirement page")]
        public void CheckIfOfferCreated()
        {
            Assert.Equal(1, this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").OffersNumber);
        }
    }
}
