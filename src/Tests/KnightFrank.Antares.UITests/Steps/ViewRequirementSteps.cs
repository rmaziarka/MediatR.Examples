namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Offer;
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
        private const string Format = "dd-MM-yyyy";
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;
        private ViewRequirementPage page;

        public ViewRequirementSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new ViewRequirementPage(this.driverContext);
            }
        }

        [When(@"User navigates to view requirement page with id")]
        public void OpenViewRequirementPageWithId()
        {
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            this.page = new ViewRequirementPage(this.driverContext).OpenViewRequirementPageWithId(requirementId.ToString());
        }

        [When(@"User clicks notes button on view requirement page")]
        public void OpenNotes()
        {
            this.page.OpenNotes().WaitForSidePanelToShow();
        }

        [When(@"User adds note on view requirement page")]
        public void InsertNotesText(Table table)
        {
            var details = table.CreateInstance<RequirementNote>();
            this.page.Notes.SetNoteText(details.Description).SaveNote();
        }

        [When(@"User clicks add viewings button on view requirement page")]
        public void ClickAddViewings()
        {
            this.page.AddViewings().WaitForSidePanelToShow();
        }

        [When(@"User selects activity on view requirement page")]
        public void SelectActivity(Table table)
        {
            var details = table.CreateInstance<Address>();

            string activity = details.PropertyName + ", " + details.PropertyNumber + " " + details.Line2;
            this.page.ActivityList.WaitForDetailsToLoad().SelectActivity(activity);
        }

        [When(@"User fills in viewing details on view requirement page")]
        public void FillInViewingDetails(Table table)
        {
            var details = table.CreateInstance<ViewingDetails>();

            this.page.Viewing
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
            List<string> attendees = table.Rows.Select(r => r[0]).ToList();

            this.page.Viewing.SelectAttendees(attendees);
        }

        [When(@"User clicks save viewing button on view requirement page")]
        public void SaveViewing()
        {
            this.page.Viewing.SaveViewing();
        }

        [When(@"User clicks (.*) viewings details on view requirement page")]
        public void OpenViewingsDetails(int position)
        {
            this.page.OpenViewingDetails(position)
                .WaitForSidePanelToShow();
        }

        [When(@"User clicks (.*) offer details on view requirement page")]
        public void OpenOffersDetails(int position)
        {
            this.page.OpenOfferDetails(position)
                .WaitForSidePanelToShow();
        }

        [When(@"User clicks edit activity button on view requirement page")]
        public void ClickEditActivity()
        {
            this.page.ViewingDetails.EditViewing();
        }

        [When(@"User clicks view activity from viewing on view requirement page")]
        public void ClickViewActivityViewing()
        {
            this.page.ViewingDetails.ClickViewLink();
        }

        [When(@"User clicks view activity from offer on view requirement page")]
        public void ClickViewActivityOffer()
        {
            this.page.OfferPreview.ClickViewLink();
        }

        [When(@"User clicks make an offer button for (.*) activity on view requirement page")]
        public void MakeAnOffer(int position)
        {
            this.page.OpenViewingActions(position)
                .CreateOffer(1)
                .WaitForSidePanelToShow();
        }

        [When(@"User clicks edit offer button for (.*) offer on view requirement page")]
        public void EditOffer(int position)
        {
            this.page.OpenOfferActions(position)
                .EditOffer(1)
                .WaitForSidePanelToShow();
        }

        [When(@"User fills in offer details on view requirement page")]
        public void FillOfferDetails(Table table)
        {
            var details = table.CreateInstance<OfferData>();

            details.OfferDate = this.scenarioContext.ContainsKey("Offer")
                ? this.scenarioContext.Get<Offer>("Offer").OfferDate.AddDays(-1).ToString(Format)
                : DateTime.UtcNow.ToString(Format);
            details.ExchangeDate = DateTime.UtcNow.AddDays(1).ToString(Format);
            details.CompletionDate = DateTime.UtcNow.AddDays(2).ToString(Format);

            this.page.Offer.SelectStatus(details.Status)
                .SetOffer(details.Offer)
                .SetOfferDate(details.OfferDate)
                .SetSpecialConditions(details.SpecialConditions)
                .SetProposedExchangeDate(details.ExchangeDate)
                .SetProposedCompletionDate(details.CompletionDate);

            this.scenarioContext.Set(details, "Offer");
        }

        [When(@"User clicks save offer button on view requirement page")]
        public void SaveOffer()
        {
            this.page.Offer.SaveOffer();
            this.page.WaitForSidePanelToHide();
        }

        [When(@"User clicks details offer link on view requirement page")]
        public void OpenViewOfferPage()
        {
            this.page.OfferPreview.ClickDetailsLink();
        }

        [When(@"User clicks add attachment button on view requirement page")]
        public void OpenAttachFilePanel()
        {
            this.page.OpenAttachFilePanel().WaitForSidePanelToShow();
        }

        [When(@"User adds (.*) file with (.*) type on view requirement page")]
        public void AddAttachment(string file, string type)
        {
            this.page.AttachFile.SelectType(type)
                .AddFiletoAttachment(file)
                .SaveAttachment();
            this.page.WaitForSidePanelToHide(60);
        }

        [When(@"User clicks attachment card on view requirement page")]
        public void OpenAttachmentPreview()
        {
            this.page.OpenAttachmentPreview().WaitForSidePanelToShow();
        }

        [Then(@"Side panel should not be displayed on view requirement page")]
        public void WaitForSidePnaleToHide()
        {
            this.page.WaitForSidePanelToHide();
        }

        [Then(@"Requirement location details on view requirement page are same as the following")]
        public void CheckResidentialSalesRequirementLocationDetails(Table table)
        {
            Dictionary<string, string> details = this.page.GetLocationRequirements();
            var expectedDetails = table.CreateInstance<Address>();

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Line2, details["Street name"]),
                () => Assert.Equal(expectedDetails.Postcode, details["Postcode"]),
                () => Assert.Equal(expectedDetails.City, details["City"]));
        }

        [Then(@"Requirement property details on view requirement page are same as the following")]
        public void CheckResidentialSalesRequirementPropertyDetails(Table table)
        {
            Dictionary<string, string> details = this.page.GetPropertyRequirements();
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
            List<string> applicants = this.page.Applicants;
            List<string> expectedApplicants =
                table.CreateSet<Contact>().Select(contact => contact.FirstName + " " + contact.Surname).ToList();

            Assert.Equal(expectedApplicants, applicants);
        }

        [Then(@"Note should be displayed in recent notes area on view requirement page")]
        public void CheckIfNoteAdded()
        {
            Assert.Equal(1, this.page.Notes.GetNumberOfNotes());
        }

        [Then(@"Notes number should increase on view requirement page")]
        public void CheckIfNotesNumberIncreased()
        {
            string notesNumber = this.page.CheckNotesNumber();
            Assert.Equal("(1)", notesNumber);
        }

        [Then(@"Viewing details on (.*) position on view requirement page are same as the following")]
        public void CheckViewing(int position, Table table)
        {
            var expectedDetails = table.CreateInstance<ViewingData>();
            List<string> actualDetails = this.page.GetViewingDetails(position);

            Verify.That(this.driverContext, () => Assert.Equal(this.page.ViewingsNumber, 1),
                () => Assert.Equal(expectedDetails.Date, actualDetails[0]),
                () => Assert.Equal(expectedDetails.Name, actualDetails[1]),
                () => Assert.Equal(expectedDetails.Time, actualDetails[2]));
        }

        [Then(@"Offer details on (.*) position on view requirement page are same as the following")]
        public void CheckOffer(int position, Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();
            List<string> actualDetails = this.page.GetOfferDetails(position);

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Details, actualDetails[0]),
                () => Assert.Equal(expectedDetails.Offer, actualDetails[1]),
                () => Assert.Equal(expectedDetails.Status, actualDetails[2]));
        }

        [Then(@"Viewing details on view requirement page are same as the following")]
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

        [Then(@"Offer details on view requirement page are same as the following")]
        public void CheckOfferInDetailsPanel(Table table)
        {
            var expectedDetails = table.CreateInstance<OfferData>();

            var offer = this.scenarioContext.Get<OfferData>("Offer");

            expectedDetails.OfferDate = offer.OfferDate;
            expectedDetails.ExchangeDate = offer.ExchangeDate;
            expectedDetails.CompletionDate = offer.CompletionDate;

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedDetails.Details, this.page.OfferPreview.Details),
                () => Assert.Equal(expectedDetails.Status, this.page.OfferPreview.Status),
                () => Assert.Equal(expectedDetails.Offer, this.page.OfferPreview.Offer),
                () => Assert.Equal(expectedDetails.OfferDate, this.page.OfferPreview.Date),
                () => Assert.Equal(expectedDetails.SpecialConditions, this.page.OfferPreview.SpecialConditions),
                () => Assert.Equal(expectedDetails.Negotiator, this.page.OfferPreview.Negotiator),
                () => Assert.Equal(expectedDetails.ExchangeDate, this.page.OfferPreview.ProposedexchangeDate),
                () => Assert.Equal(expectedDetails.CompletionDate, this.page.OfferPreview.ProposedCompletionDate));
        }

        [Then(@"View requirement page should be displayed")]
        public void CheckIfViewRequirementPresent()
        {
            Assert.True(this.page.IsViewRequirementFormPresent());
        }

        [Then(@"Activity details on view requirement page are same as the following")]
        public void CheckOfferActivity(Table table)
        {
            var details = table.CreateInstance<OfferData>();
            Assert.Equal(details.Details, this.page.Offer.Details);
        }

        [Then(@"New offer should be created and displayed on view requirement page")]
        public void CheckIfOfferCreated()
        {
            Assert.Equal(1, this.page.OffersNumber);
        }

        [Then(@"New requirement should be created")]
        public void CheckIfNewResidentialSalesRequirementCreated()
        {
            this.page.WaitForDetailsToLoad();
            var date = this.scenarioContext.Get<DateTime>("RequirementDate");
            Assert.Equal(date.ToString("MMMM d, yyyy"), this.page.CreateDate);
        }

        [Then(@"Attachment should be displayed on view requirement page")]
        public void CheckIfAttachmentIsDisplayed(Table table)
        {
            Attachment actual = this.page.AttachmentDetails;
            var expected = table.CreateInstance<Attachment>();
            expected.Date = DateTime.UtcNow.ToString(Format);

            actual.ShouldBeEquivalentTo(expected);
        }

        [Then(@"Attachment details on attachment preview page are the same like on view requirement page")]
        public void ChackAttachmentDetails()
        {
            Attachment actual = this.page.AttachmentPreview.GetAttachmentDetails();
            actual.Date = actual.Date.Split(',')[0];
            Attachment expected = this.page.AttachmentDetails;
            expected.User = "John Smith";

            actual.ShouldBeEquivalentTo(expected);
        }

        [Then(@"Requirement attachment (.*) should be downloaded")]
        public void ThenAttachmentShouldBeDownloaded(string attachmentName)
        {
            FileInfo fileInfo = this.page.AttachmentPreview.GetDownloadedAttachmentInfo();

            Verify.That(this.driverContext,
                () => Assert.Equal(attachmentName.ToLower(), fileInfo.Name),
                () => Assert.Equal("." + attachmentName.Split('.')[1], fileInfo.Extension));
        }

        [Then(@"User closes attachment preview page on view requirement page")]
        public void CloseAttachmentPreviewPanel()
        {
            this.page.AttachmentPreview.CloseAttachmentPreviewPage();
            this.page.WaitForSidePanelToHide();
        }
    }
}
