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

        [When(@"User clicks notes button on residential sales requirement page")]
        public void OpenNotes()
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").OpenNotes();
        }

        [When(@"User adds text to add note area on notes page")]
        public void InsertNotesText(Table table)
        {
            var details = table.CreateInstance<RequirementNote>();
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").Notes.SetNoteText(details.Description);
        }

        [When(@"User clicks save button on notes page")]
        public void SaveNotes()
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").Notes.SaveNote();
        }

        [Then(@"residential sales requirement location details are same as the following")]
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

        [Then(@"residential sales requirement property details are same as the following")]
        public void CheckResidentialSalesRequirementPropertyDetails(Table table)
        {
            Dictionary<string, string> details =
                this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage")
                    .GetPropertyRequirements();
            var expectedDetails = table.CreateInstance<Requirement>();

            Verify.That(this.driverContext,
                () =>
                    Assert.True(details["Price"].Contains(expectedDetails.MinPrice + " - " + expectedDetails.MaxPrice + " GBP"),
                        "Prices are different"),
                () =>
                    Assert.True(details["Bedrooms"].Contains(expectedDetails.MinBedrooms + " - " + expectedDetails.MaxBedrooms),
                        "Number of bedrooms is different"),
                () =>
                    Assert.True(
                        details["Reception rooms"].Contains(expectedDetails.MinReceptionRooms + " - " +
                                                            expectedDetails.MaxReceptionRooms),
                        "Number of reception rooms is different"),
                () =>
                    Assert.True(
                        details["Bathrooms"].Contains(expectedDetails.MinBathrooms + " - " + expectedDetails.MaxBathrooms),
                        "Number of bathrooms is different"),
                () =>
                    Assert.True(
                        details["Parking spaces"].Contains(expectedDetails.MinParkingSpaces + " - " +
                                                           expectedDetails.MaxParkingSpaces),
                        "Number of parking spaces is different"),
                () =>
                    Assert.True(details["Area"].Contains(expectedDetails.MinArea + " - " + expectedDetails.MaxArea + " sq ft"),
                        "Areas are different"),
                () =>
                    Assert.True(
                        details["Land area"].Contains(expectedDetails.MinLandArea + " - " + expectedDetails.MaxLandArea + " sq ft"),
                        "Land areas are different"),
                () => Assert.Equal(expectedDetails.Description, details["Requirement description"]));
        }

        [Then(@"residential sales requirement applicants are same as the following")]
        public void CheckResidentialSalesRequirementApplicants(Table table)
        {
            List<string> applicants =
                this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage")
                    .GetApplicants();
            List<string> expectedApplicants =
                table.CreateSet<Contact>().Select(contact => contact.FirstName + " " + contact.Surname).ToList();

            Assert.Equal(expectedApplicants, applicants);
        }

        [Then(@"residential sales requirement create date is equal to today")]
        public void CheckResidentialSalesRequirementCretaeDate()
        {
            var date = this.scenarioContext.Get<DateTime>("RequirementDate");

            Assert.Equal(date.ToString("MMMM d, yyyy"),
                this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage")
                    .GetRequirementCreateDate());
        }

        [Then(@"Add note area is cleard on notes page")]
        public void CheckIfAddNoteAreaIsEmpty()
        {
            this.scenarioContext.Get<ViewRequirementPage>("ViewRequirementPage").Notes.CheckIfAddNoteIsCleared();
        }
    }
}
