namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class CreateRequirementSteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;

        public CreateRequirementSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [Given(@"Requirement for GB is created in database")]
        public void CreateRequirementInDb(Table table)
        {
            var requirement = table.CreateInstance<Requirement>();
            KnightFrankContext dataContext = DatabaseExtensions.GetDataContext();

            // Get country and address form id
            Guid countryId = dataContext.Countries.Single(x => x.IsoCode == "GB").Id;
            Guid enumTypeId = dataContext.EnumTypeItems.Single(e => e.Code == "Requirement").Id;
            Guid addressFormId =
                dataContext.AddressFormEntityTypes.Single(
                    afe => afe.AddressForm.CountryId == countryId && afe.EnumTypeItemId == enumTypeId).AddressFormId;

            // Set requirement details
            requirement.CreateDate = DateTime.UtcNow;
            requirement.Contacts = this.scenarioContext.Get<List<Contact>>("ContactsList");
            requirement.Address = new Address
            {
                Line2 = "56 Belsize Park",
                Postcode = "NW3 4EH",
                City = "London",
                AddressFormId = addressFormId,
                CountryId = countryId
            };

            dataContext.Requirements.Add(requirement);
            dataContext.CommitAndClose();
            this.scenarioContext.Set(requirement, "Requirement");
        }

        [When(@"User navigates to create requirement page")]
        public void OpenCreateRequirementPage()
        {
            CreateRequirementPage page =
                new CreateRequirementPage(this.driverContext).OpenCreateRequirementPage();
            this.scenarioContext["CreateRequirementPage"] = page;
        }

        [When(@"User fills in location details on create requirement page")]
        public void SetLocationRequirementDetails(Table table)
        {
            var page = this.scenarioContext.Get<CreateRequirementPage>("CreateRequirementPage");

            var details = table.CreateInstance<Address>();

            page.AddressTemplate.SelectPropertyCountry(table.Rows[0]["Country"])
                .SetPropertyCity(details.City)
                .SetPropertyAddressLine2(details.Line2)
                .SetPropertyPostCode(details.Postcode);
        }

        [When(@"User fills in property details on create requirement page")]
        public void SetPropertyRequirementDetails(Table table)
        {
            var page = this.scenarioContext.Get<CreateRequirementPage>("CreateRequirementPage");
            var details = table.CreateInstance<Requirement>();

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

        [When(@"User selects contacts on create requirement page")]
        public void SelectContactsForRequirement(Table table)
        {
            var page = this.scenarioContext.Get<CreateRequirementPage>("CreateRequirementPage");

            page.AddNewApllicantForResidentialSalesRequirement();

            IEnumerable<Contact> contacts = table.CreateSet<Contact>();

            foreach (Contact contact in contacts)
            {
                page.ContactsList.WaitForContactsListToLoad().SelectContact(contact.FirstName, contact.Surname);
            }
            page.ContactsList.SaveContact().WaitForContactListToHide();
        }

        [When(@"User clicks save button on create requirement page")]
        public void SaveNewResidentialSalesRequirement()
        {
            this.scenarioContext.Get<CreateRequirementPage>("CreateRequirementPage")
                .SaveRequirement();
            this.scenarioContext["RequirementDate"] = DateTime.UtcNow;
        }

        [Then(@"New requirement should be created")]
        public void CheckIfNewResidentialSalesRequirementCreated()
        {
            var page = new ViewRequirementPage(this.driverContext);
            page.WaitForDetailsToLoad();
            this.scenarioContext["ViewRequirementPage"] = page;
        }

        [Then(@"List of applicants should contain following contacts")]
        public void CheckApplicantsList(Table table)
        {
            var page = this.scenarioContext.Get<CreateRequirementPage>("CreateRequirementPage");

            List<string> applicants =
                table.CreateSet<Contact>().Select(contact => contact.FirstName + " " + contact.Surname).ToList();

            List<string> selectedApplicants = page.GetApplicants();

            Assert.Equal(applicants.Count, selectedApplicants.Count);
            applicants.ShouldBeEquivalentTo(selectedApplicants);
        }
    }
}
