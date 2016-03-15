namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model;
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
            NewResidentialSalesRequirementPage newResidentialSalesRequirementPage =
                new NewResidentialSalesRequirementPage(this.driverContext).OpenNewResidentialSalesRequirementPage();
            this.scenarioContext["NewResidentialSalesRequirementPage"] = newResidentialSalesRequirementPage;
        }

        [When(@"User fills in location details on create residential sales requirement page")]
        public void SetLocationRequirementDetails(Table table)
        {
            var newResidentialSalesRequirementPage =
                this.scenarioContext.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage");

            var details = table.CreateInstance<Address>();

            //TODO select country - SelectPropertyCountry(details.Country)
            newResidentialSalesRequirementPage.AddressTemplate.SetPropertyName(details.PropertyName)
                                              .SetPropertyNumber(details.PropertyNumber)
                                              .SetPropertyAddressLine1(details.Line1).SetPropertyAddressLine2(details.Line2)
                                              .SetPropertyPostCode(details.Postcode)
                                              .SetPropertyCity(details.City)
                                              .SetPropertyCounty(details.County);
        }

        [When(@"User fills in property details on create residential sales requirement page")]
        public void SetPropertyRequirementDetails(Table table)
        {
            var newResidentialSalesRequirementPage =
                this.scenarioContext.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage");

            var details = table.CreateInstance<Requirement>();

            //TODO select property type - SelectPropertyType(details.Type)
            newResidentialSalesRequirementPage.SetPropertyMinPrice(details.MinPrice.ToString())
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
            var newResidentialSalesRequirementPage =
                this.scenarioContext.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage");

            newResidentialSalesRequirementPage.AddNewApllicantForResidentialSalesRequirement();

            IEnumerable<Contact> contacts = table.CreateSet<Contact>();

            foreach (Contact contact in contacts)
            {
                newResidentialSalesRequirementPage.ContactsList.SelectContact(contact.FirstName, contact.Surname);
            }
            newResidentialSalesRequirementPage.ContactsList.SaveContact();
        }

        [When(@"User clicks save button on create residential sales requirement page")]
        public void SaveNewResidentialSalesRequirement()
        {
            this.scenarioContext.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage")
                .SaveNewResidentialSalesRequirement();
        }

        [Then(@"New residential sales requirement should be created")]
        public void CheckIfNewResidentialSalesRequirementCreated()
        {
            //TODO implement check if sales requirement was created
        }

        [Then(@"list of applicants should contain following contacts")]
        public void CheckApplicantsList(Table table)
        {
            var newResidentialSalesRequirementPage =
                this.scenarioContext.Get<NewResidentialSalesRequirementPage>("NewResidentialSalesRequirementPage");

            List<string> applicants =
                table.CreateSet<Contact>().Select(contact => contact.FirstName + " " + contact.Surname).ToList();

            List<string> selectedApplicants = newResidentialSalesRequirementPage.GetApplicants();

            Assert.Equal(applicants.Count, selectedApplicants.Count);
            applicants.Should().BeEquivalentTo(selectedApplicants);
        }
    }
}
