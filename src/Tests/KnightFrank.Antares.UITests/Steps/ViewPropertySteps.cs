namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using FluentAssertions;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.UITests.Pages;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ViewPropertySteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;
        private ViewPropertyPage page;

        public ViewPropertySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new ViewPropertyPage(this.driverContext);
            }
        }

        [When(@"User navigates to view property page with id")]
        public void OpenViewRequirementPageWithId()
        {
            Guid propertyId = this.scenarioContext.Get<Property>("Property").Id;
            this.page = new ViewPropertyPage(this.driverContext).OpenViewPropertyPageWithId(propertyId.ToString());
        }

        [When(@"User clicks add activites button on view property page")]
        public void ClickAddActivityButton()
        {
            this.page.AddActivity().WaitForSidePanelToShow();
        }

        [When(@"User clicks edit property button on view property page")]
        public void WhenUserClicksEditButtonOnCreatePropertyPage()
        {
            this.page.EditProperty();
        }

        [When(@"User selects contacts for ownership on view property page")]
        public void SelectContactsForOwnership(Table table)
        {
            this.page.SetOwnership().WaitForSidePanelToShow();
            IEnumerable<Contact> contacts = table.CreateSet<Contact>();

            foreach (Contact contact in contacts)
            {
                this.page.ContactsList.WaitForContactsListToLoad().SelectContact(contact.FirstName, contact.Surname);
            }
            this.page.ContactsList.ConfigureOwnership();
        }

        [When(@"User clicks activity details link on view property page")]
        public void OpenActivitiesPreview()
        {
            this.page.OpenActivityDetails().WaitForSidePanelToShow();
        }

        [When(@"User clicks view activity link from activity on view property page")]
        public void OpenViewActivityPage()
        {
            this.page.ActivityPreview.ClickViewActivity();
        }

        [When(@"User selects (.*) activity type on create activity page")]
        public void SelectActivityType(string type)
        {
            this.page.Activity.SelectActivityType(type);
        }

        [When(@"User selects (.*) activity status on create activity page")]
        public void SelectActivityStatus(string status)
        {
            this.page.Activity.SelectActivityStatus(status);
        }

        [When(@"User clicks save button on create activity page")]
        public void ClickSaveButtonOnActivityPanel()
        {
            this.page.Activity.SaveActivity();
            this.page.WaitForSidePanelToHide();
        }

        [When(@"User fills in ownership details on view property page")]
        public void FillInOwnershipDetails(Table table)
        {
            var details = table.CreateInstance<OwnershipDetails>();

            if (Convert.ToBoolean(details.Current))
            {
                this.page.Ownership.SetCurrentOwnership()
                    .SelectOwnershipType(details.Type);
                if (!string.IsNullOrEmpty(details.PurchasePrice))
                {
                    this.page.Ownership.SetPurchasePrice(details.PurchasePrice);
                }
                if (!string.IsNullOrEmpty(details.PurchaseDate))
                {
                    this.page.Ownership.SetPurchaseDate(details.PurchaseDate);
                }
            }
            else
            {
                this.page.Ownership.SetOwnership()
                    .SelectOwnershipType(details.Type);
                if (!string.IsNullOrEmpty(details.PurchasePrice))
                {
                    this.page.Ownership.SetPurchasePrice(details.PurchasePrice);
                }
                if (!string.IsNullOrEmpty(details.SellPrice))
                {
                    this.page.Ownership.SetSellPrice(details.SellPrice);
                }
                if (!string.IsNullOrEmpty(details.PurchaseDate))
                {
                    this.page.Ownership.SetPurchaseDate(details.PurchaseDate);
                }
                if (!string.IsNullOrEmpty(details.SellDate))
                {
                    this.page.Ownership.SetSellDate(details.SellDate);
                }
            }
            this.page.Ownership.SaveOwnership();
            this.page.WaitForSidePanelToHide();
        }

        [When(@"User clicks add area breakdown button on view property page")]
        public void AddArea()
        {
            this.page.CreateAreaBreakdown().WaitForSidePanelToShow();
        }

        [When(@"User fills in area details on view property page")]
        [When(@"User updates area details on view property page")]
        public void AddAreaDetails(Table table)
        {
            List<PropertyAreaBreakdown> areaBreakdowns = table.CreateSet<PropertyAreaBreakdown>().ToList();

            var place = 1;
            foreach (PropertyAreaBreakdown areaBreakdown in areaBreakdowns)
            {
                this.page.Area.SetAreaDetails(areaBreakdown.Name, areaBreakdown.Size, place++);
            }
        }

        [When(@"User clicks save area button on view property page")]
        public void SaveAreas()
        {
            this.page.Area.SaveArea();
            this.page.WaitForSidePanelToHide();
        }

        [When(@"User clicks edit area button for (.*) area on view property page")]
        public void EditArea(int position)
        {
            this.page.OpenAreaActions(position)
                .EditArea(1)
                .WaitForSidePanelToShow();
        }

        [Then(@"Activity details are set on view property page")]
        public void CheckActivityDetails(Table table)
        {
            var details = table.CreateInstance<ActivityDetails>();

            Verify.That(this.driverContext,
                () => Assert.Equal(details.Vendor, this.page.ActivityVendor),
                () => Assert.Equal(details.Status, this.page.ActivityStatus),
                () => Assert.Equal(details.Type, this.page.ActivityType),
                () => Assert.Equal(DateTime.UtcNow.ToString("dd-MM-yyyy"), this.page.ActivityDate));
        }

        [Then(@"Property should be updated with address details")]
        [Then(@"Property should be displayed with address details")]
        [Then(@"New property should be created with address details")]
        public void CheckIfPropertyCreated(Table table)
        {
            foreach (string field in table.Rows.SelectMany(row => row.Values))
            {
                Assert.True(field.Equals(string.Empty)
                    ? this.page.IsAddressDetailsNotVisible(field)
                    : this.page.IsAddressDetailsVisible(field));
            }
        }

        [Then(@"New property should be created with (.*) property type and following attributes")]
        [Then(@"Property should be updated with (.*) property type and following attributes")]
        public void CheckPropertyType(string propertyType, Table table)
        {
            var details = table.CreateInstance<PropertyDetails>();

            Dictionary<string, string> actualDetails = this.page.GetPropertyDetails();
            Dictionary<string, string> expectedDetails =
                details.GetType()
                       .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                       .ToDictionary(prop => prop.Name.ToLower(), prop => prop.GetValue(details, null))
                       .Where(item => item.Value != null)
                       .ToDictionary(x => x.Key, x => x.Value.ToString());

            Assert.Equal(propertyType, this.page.PropertyType);
            actualDetails.ShouldBeEquivalentTo(expectedDetails);
        }

        [Then(@"Ownership details should contain following data on view property page")]
        public void CheckOwnershipContacts(Table table)
        {
            IEnumerable<OwnershipDetails> details = table.CreateSet<OwnershipDetails>();

            foreach (OwnershipDetails ownershipDetails in details)
            {
                string contact = this.page.GetOwnershipContact(ownershipDetails.Position);
                string currentOwnershipDetails = this.page.GetOwnershipDetails(ownershipDetails.Position);
                string expectdOwnershipDetails = ownershipDetails.Type.ToUpper();

                if (!string.IsNullOrEmpty(ownershipDetails.PurchaseDate) && string.IsNullOrEmpty(ownershipDetails.SellDate))
                {
                    expectdOwnershipDetails += " " + ownershipDetails.PurchaseDate + " -";
                }
                else if (string.IsNullOrEmpty(ownershipDetails.PurchaseDate) && !string.IsNullOrEmpty(ownershipDetails.SellDate))
                {
                    expectdOwnershipDetails += " - " + ownershipDetails.SellDate;
                }
                else if (string.IsNullOrEmpty(ownershipDetails.PurchaseDate) && string.IsNullOrEmpty(ownershipDetails.SellDate))
                {
                    expectdOwnershipDetails += " -";
                }
                else if (!string.IsNullOrEmpty(ownershipDetails.PurchaseDate) && !string.IsNullOrEmpty(ownershipDetails.SellDate))
                {
                    expectdOwnershipDetails += " " + ownershipDetails.PurchaseDate + " - " + ownershipDetails.SellDate;
                }

                Assert.Equal(ownershipDetails.ContactName + " " + ownershipDetails.ContactSurname, contact);
                Assert.Equal(expectdOwnershipDetails, currentOwnershipDetails);
            }
        }

        [Then(@"View property page should be displayed")]
        public void CheckIfViewPropertyPresent()
        {
            Assert.True(this.page.IsViewPropertyFormPresent());
        }

        [Then(@"Activity details are set on create activity page")]
        public void CheckActivityDetailsonActivityPanel(Table table)
        {
            var details = table.CreateInstance<ActivityDetails>();

            Verify.That(this.driverContext,
                () => Assert.Equal(details.Vendor, this.page.Activity.GetActivityVendor()),
                () => Assert.Equal(details.Status, this.page.Activity.GetActivityStatus()));
        }

        [Then(@"Characteristics are displayed on view property page")]
        public void CheckCharacteristics(Table table)
        {
            Dictionary<string, string> actualDetails = this.page.GetCharacteristics();
            Dictionary<string, string> characteristics = table.CreateSet<Characteristic>()
                                                              .ToDictionary(x => x.Name, x => x.Comment);

            actualDetails.ShouldBeEquivalentTo(characteristics);
        }

        [Then(@"Area breakdown order is following on view property page")]
        public void CheckAreasOrder(Table table)
        {
            List<PropertyAreaBreakdown> expectedAreas = table.CreateSet<PropertyAreaBreakdown>().ToList();
            List<PropertyAreaBreakdown> actualAreas = this.page.GetAreas();
            actualAreas.Should().Equal(expectedAreas, (c1, c2) =>
                c1.Name.Equals(c2.Name) &&
                c1.Order.Equals(c2.Order) &&
                c1.Size.Equals(c2.Size));
        }

        [Then(@"Property created success message should be displayed")]
        public void CheckIfSuccessMessageDisplayed()
        {
            Verify.That(this.driverContext,
                () => Assert.True(this.page.IsSuccessMessageDisplayed()),
                () => Assert.Equal("New property has been created", this.page.SuccessMessage));
            this.page.WaitForSuccessMessageToHide();
        }
    }
}
