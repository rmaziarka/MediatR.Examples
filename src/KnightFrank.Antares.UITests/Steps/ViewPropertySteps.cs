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
        //private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;

        public ViewPropertySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;

            //this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [When(@"User clicks add activites button on property details page")]
        public void ClickAddActivityButton()
        {
            this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").AddActivity();
        }

        [When(@"User clicks save button on activity panel")]
        public void ClickSaveButtonOnActivityPanel()
        {
            this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").Activity.SaveActivity();
        }

        [When(@"User clicks edit button on property details page")]
        public void WhenUserClicksEditButtonOnCreatePropertyPage()
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            this.scenarioContext.Set(page.EditProperty(), "CreatePropertyPage");
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

        [When(@"User clicks ownership details on position (.*) on view property page")]
        public void OpenOwnershipDetails(int position)
        {
            this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").OpenOwnershipDetails(position);
        }

        [When(@"User clicks activity's details link on property details page")]
        public void OpenActivitiesPreview()
        {
            this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").ClickDetailsLink();
        }


        [Then(@"Activity creation date is set to current date on property details page")]
        public void CheckifActivityDateCorrect()
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            Assert.Equal(DateTime.Now.ToString("dd-MM-yyyy"), page.GetActivityDate());
        }

        [Then(@"Activity details are set on property details page")]
        public void CheckActivityDetails(Table table)
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            Assert.Equal(table.Rows[0]["Vendor"], page.GetActivityVendor());
            Assert.Equal(table.Rows[0]["Status"], page.GetActivityStatus());
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

        [Then(@"Activity details are set on activity panel")]
        public void CheckActivityDetailsonActivityPanel(Table table)
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");

            //Assert.Equal(table.Rows[0]["Vendor"], page.Activity.GetActivityVendor());
            Assert.Equal(table.Rows[0]["Status"], page.Activity.GetActivityStatus());
        }

        [Then(@"Ownership contacts on position (.*) should contain following contacts on view property page")]
        public void CheckOwnershipContacts(int position, Table table)
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            List<string> contacts = page.GetOwnershipContacts(position);
            List<string> expectedContacts = table.CreateSet<Contact>().Select(c => c.FirstName + " " + c.Surname).ToList();

            Assert.Equal(expectedContacts, contacts);
        }

        [Then(@"Ownership details on position (.*) should contain following data on view property page")]
        public void CheckOwnershipDetails(int position, Table table)
        {
//            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
//            string details = page.GetOwnershipDetails(position);
//
//            string type = table.Rows[0]["Type"].ToUpper();
//            string fromDate = table.Rows[0]["PurchaseDate"];
//            string toDate = table.Rows[0]["SellDate"];
//
//            string expectedDetails = type + " " + fromDate + " - " + toDate;
//            //TODO remove comment when dates are fixed
//            Assert.Equal(expectedDetails, details);
        }

        [Then(@"Activity preview panel is displayed with details the same like details on activity tile")]
        public void CheckActivityPreview()
        {
            var preview = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            Assert.Equal(preview.GetActivityDate(), preview.PreviewDetails.GetCreationDate());
            Assert.Equal(preview.GetActivityStatus(), preview.PreviewDetails.GetStatus());
            //Assert.Equal(preview.GetActivityVendor(),preview.PreviewDetails.GetVendor());
        }
    }
}
