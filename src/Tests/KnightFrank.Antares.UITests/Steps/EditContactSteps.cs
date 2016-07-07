using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace KnightFrank.Antares.UITests.Steps
{
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public sealed class EditContactSteps
    {
        private readonly DriverContext driverContext;
        private readonly EditContactPage page;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;

        public EditContactSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new EditContactPage(this.driverContext);
            }
        }

        [When(@"User fills in contact details on edit contact page")]
        [Then(@"User fills in contact details on edit contact page")]
        public void EditContact(Table table)
        {
            var contact = table.CreateInstance<Contact>();

            this.page.SetFirstName(contact.FirstName)
                .SetLastName(contact.LastName)
                .SetTitle(contact.Title);
        }

        [When(@"User clicks save button on edit contact page")]
        [Then(@"User clicks save button on edit contact page")]
        public void SaveEditedContact()
        {
            this.page.SaveEditedContact();
        }

        [Then(@"Edit contact page should be displayed")]
        public void CheckEditContactPage()
        {
            Assert.True(this.page.IsEditContactFormPresent());
        }
    }
}
