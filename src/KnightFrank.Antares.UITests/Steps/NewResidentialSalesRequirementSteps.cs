namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class NewResidentialSalesRequirementSteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;

        public NewResidentialSalesRequirementSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [Given(@"User navigates to create residential sales requirement page")]
        [When(@"User navigates to create residential sales requirement page")]
        public void OpenNewResidentialSalesRequirementPage()
        {
            NewResidentialSalesRequirementPage page =
                new NewResidentialSalesRequirementPage(this.driverContext).OpenNewResidentialSalesRequirementPage();
            this.scenarioContext["NewResidentialSalesRequirementPage"] = page;
        }

        [When(@"User fills in location details on create residential sales requirement page")]
        public void SetLocationRequirementDetails(Table table)
        {
            var page = this.scenarioContext.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage");

            var details = table.CreateInstance<Address>();

            page.AddressTemplate.SelectPropertyCountry(table.Rows[0]["Country"])
                .SetPropertyName(details.PropertyName)
                .SetPropertyNumber(details.PropertyNumber)
                .SetPropertyAddressLine1(details.Line1)
                .SetPropertyAddressLine2(details.Line2)
                .SetPropertyAddressLine3(details.Line3)
                .SetPropertyPostCode(details.Postcode)
                .SetPropertyCity(details.City)
                .SetPropertyCounty(details.County);
        }

        [When(@"User fills in property details on create residential sales requirement page")]
        public void SetPropertyRequirementDetails(Table table)
        {
            var page = this.scenarioContext.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage");

            var details = table.CreateInstance<Requirement>();

            //TODO select property type - SelectPropertyType(details.Type)
            page.SetPropertyMinPrice(details.MinPrice.ToString())
                .SetPropertyMaxPrice(details.MaxPrice.ToString())
                .SetPropertyBedroomsMin(details.MinBedrooms.ToString())
                .SetPropertyBedroomMax(details.MaxBedrooms.ToString())
                .SetPropertyReceptionRoomsMin(details.MinReceptionRooms.ToString())
                .SetPropertyReceptionRoomsMax(details.MaxReceptionRooms.ToString())
                .SetPropertyBathroomsMin(details.MinBathrooms.ToString())
                .SetPropertyBathroomsMax(details.MaxBathrooms.ToString())
                .SetPropertyParkingSpacesMin(details.MinParkingSpaces.ToString())
                .SetPropertyParkingSpacesMax(details.MaxParkingSpaces.ToString())
                .SetPropertyAreaMin(details.MinArea.ToString())
                .SetPropertyAreaMax(details.MaxArea.ToString())
                .SetPropertyLandAreaMin(details.MinLandArea.ToString())
                .SetPropertyLandAreaMax(details.MaxLandArea.ToString())
                .SetPropertyRequirementsNote(details.Description);
        }

        [When(@"User selects contacts on create residential sales requirement page")]
        public void SelectContactsForRequirement(Table table)
        {
            var page = this.scenarioContext.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage");

            page.AddNewApllicantForResidentialSalesRequirement();

            IEnumerable<Contact> contacts = table.CreateSet<Contact>();

            foreach (Contact contact in contacts)
            {
                page.ContactsList.WaitForContactsListToLoad().SelectContact(contact.FirstName, contact.Surname);
            }
            page.ContactsList.SaveContact().WaitForContactListToHide();
        }

        [When(@"User clicks save button on create residential sales requirement page")]
        public void SaveNewResidentialSalesRequirement()
        {
            this.scenarioContext.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage")
                .SaveNewResidentialSalesRequirement();
            this.scenarioContext["RequirementDate"] = DateTime.UtcNow;
        }

        [Then(@"New residential sales requirement should be created")]
        public void CheckIfNewResidentialSalesRequirementCreated()
        {
            var page = new ResidentialSalesRequirementDetailsPage(this.driverContext);
            page.WaitForDetailsToLoad();
            this.scenarioContext["ResidentialSalesRequirementDetailsPage"] = page;
        }

        [Then(@"list of applicants should contain following contacts")]
        public void CheckApplicantsList(Table table)
        {
            var page = this.scenarioContext.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage");

            List<string> applicants =
                table.CreateSet<Contact>().Select(contact => contact.FirstName + " " + contact.Surname).ToList();

            List<string> selectedApplicants = page.GetApplicants();

            Assert.Equal(applicants.Count, selectedApplicants.Count);
            applicants.Should().BeEquivalentTo(selectedApplicants);
        }
    }
}
