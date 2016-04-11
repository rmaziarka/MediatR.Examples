namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.UITests.Pages;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ViewPropertySteps
    {
        private readonly ScenarioContext scenarioContext;

        public ViewPropertySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
        }

        [When(@"User clicks add activites button on view property page")]
        public void ClickAddActivityButton()
        {
            this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").AddActivity();
        }

        [When(@"User clicks edit button on view property page")]
        public void WhenUserClicksEditButtonOnCreatePropertyPage()
        {
            this.scenarioContext.Set(this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").EditProperty(),
                "CreatePropertyPage");
        }

        [When(@"User selects contacts for ownership on view property page")]
        public void SelectContactsForOwnership(Table table)
        {
            ViewPropertyPage page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").SetOwnership();
            IEnumerable<Contact> contacts = table.CreateSet<Contact>();

            foreach (Contact contact in contacts)
            {
                page.ContactsList.WaitForContactsListToLoad().SelectContact(contact.FirstName, contact.Surname);
            }
            page.ContactsList.ConfigureOwnership();
        }

        [When(@"User clicks activity's details link on view property page")]
        public void OpenActivitiesPreview()
        {
            this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").OpenActivityDetails();
        }

        [When(@"User clicks view activity link on activity preview page")]
        public void OpenViewActivityPage()
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            this.scenarioContext.Set(page.PreviewDetails.ClickViewActivity(), "ViewActivityPage");
        }

        [Then(@"Activity creation date is set to current date on view property page")]
        public void CheckifActivityDateCorrect()
        {
            Assert.Equal(DateTime.Now.ToString("dd-MM-yyyy"),
                this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").GetActivityDate());
        }

        [Then(@"Activity details are set on view property page")]
        public void CheckActivityDetails(Table table)
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            var details = table.CreateInstance<ActivityDetails>();

            Assert.Equal(details.Vendor, page.GetActivityVendor());
            Assert.Equal(details.Status, page.GetActivityStatus());
        }

        [Then(@"Property should be updated with address details")]
        [Then(@"New property should be created with address details")]
        public void CheckIfPropertyCreated(Table table)
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");

            foreach (string field in table.Rows.SelectMany(row => row.Values))
            {
                Assert.True(field.Equals(string.Empty)
                    ? page.IsAddressDetailsNotVisible(field)
                    : page.IsAddressDetailsVisible(field));
            }
        }

        [Then(@"Ownership details should contain following data on view property page")]
        public void CheckOwnershipContacts(Table table)
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            IEnumerable<OwnershipDetails> details = table.CreateSet<OwnershipDetails>();

            foreach (OwnershipDetails ownershipDetails in details)
            {
                string contact = page.GetOwnershipContact(ownershipDetails.Position);
                string currentOwnershipDetails = page.GetOwnershipDetails(ownershipDetails.Position);
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

        [Then(@"View property page is displayed")]
        public void CheckIfViewPropertyPresent()
        {
            Assert.True(this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").CheckIfViewPropertyPresent());
        }
    }
}
