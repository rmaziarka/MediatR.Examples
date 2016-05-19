namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ViewRequirementSteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;

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
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").OpenNotes();
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
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").AddViewings();
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
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").Viewing
                .SaveViewing()
                .WaitForViewingDetailsToHide();
        }

        [When(@"User clicks (.*) viewings details link on view requirement page")]
        public void OpenViewingsDetails(int position)
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").OpenViewingDetails(position);
        }

        [When(@"User clicks edit activity button on view requirement page")]
        public void ClickEditActivity()
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").ViewingDetails.EditViewing();
        }

        [When(@"User clicks view activity on view requirement page")]
        public void ClickViewActivity()
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").ViewingDetails.ClickViewLink();
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
                this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage")
                    .GetApplicants();
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

        [Then(@"View requirement page is displayed")]
        public void CheckIfViewRequirementPresent()
        {
            var page = new ViewRequirementPage(this.driverContext);
            Assert.True(page.IsViewRequirementFormPresent());
            this.scenarioContext.Set(page, "ViewActivityPage");
        }

    }
}
