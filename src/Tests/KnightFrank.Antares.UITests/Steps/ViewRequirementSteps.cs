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
            ViewRequirementPage page = new ViewRequirementPage(this.driverContext).OpenViewRequirementPageWithId(requirementId.ToString());
            this.scenarioContext.Set(page, "ViewRequirementPage");
        }

        [When(@"User clicks notes button on residential sales requirement page")]
        public void OpenNotes()
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").OpenNotes();
        }

        [When(@"User adds note on residential sales requirement page")]
        public void InsertNotesText(Table table)
        {
            var details = table.CreateInstance<RequirementNote>();
            var page = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage");
            page.Notes.SetNoteText(details.Description).SaveNote();
        }

        [Then(@"Requirement location details are same as the following")]
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

        [Then(@"Requirement property details are same as the following")]
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

        [Then(@"Requirement applicants are same as the following")]
        public void CheckResidentialSalesRequirementApplicants(Table table)
        {
            List<string> applicants =
                this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage")
                    .GetApplicants();
            List<string> expectedApplicants =
                table.CreateSet<Contact>().Select(contact => contact.FirstName + " " + contact.Surname).ToList();

            Assert.Equal(expectedApplicants, applicants);
        }

        [Then(@"Requirement create date is equal to today")]
        public void CheckResidentialSalesRequirementCretaeDate()
        {
            var date = this.scenarioContext.Get<DateTime>("RequirementDate");

            Assert.Equal(date.ToString("MMMM d, yyyy"),
                this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage")
                    .GetRequirementCreateDate());
        }

        [Then(@"Note is displayed in recent notes area on residential sales requirement page")]
        public void CheckIfNoteAdded()
        {
            Assert.Equal(1, this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").Notes.GetNumberOfNotes());
        }

        [Then(@"Notes number increased on residential sales requirement page")]
        public void CheckIfNotesNumberIncreased()
        {
            string notesNumber = this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").CheckNotesNumber();
            Assert.Equal("(1)", notesNumber);
        }
    }
}
