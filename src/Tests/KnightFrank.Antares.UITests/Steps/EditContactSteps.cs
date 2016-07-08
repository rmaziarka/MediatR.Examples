namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public sealed class EditContactSteps
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
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
        public void EditContact(Table table)
        {
            var contact = table.CreateInstance<Contact>();

            this.page.SetFirstName(contact.FirstName)
                .SetLastName(contact.LastName)
                .SetTitle(contact.Title);
        }

        [When(@"User clicks save button on edit contact page")]
        public void SaveEditedContact()
        {
            this.page.SaveEditedContact();
        }

        [When(@"User selects primary negotiator to (.*) on edit contact page")]
        public void SelectPrimaryNegotiator(string user)
        {
            this.page.SetPrimaryNegotiatorName(user);
        }

        [When(@"User selects secondary negotiatiors on edit contact page")]
        public void CreateSecondaryContactUsers(Table table)
        {
            List<User> users = table.CreateSet<User>().ToList();

            foreach (User user in users)
            {
                this.page.SetSecondaryNegotiatorName(user.FirstName + ' ' + user.LastName);
            }
        }

        [When(@"User sets (.*) secondary negotiator as lead negotiator on edit contact page")]
        public void SetLeadNegotiatorFromSecondaryNegotiator(int position)
        {
            this.page.SetSecondaryNegotiatorAsLeadNegotiator(position);
        }

        [When(@"User removes (.*) secondary negotiator on edit contact page")]
        public void RemoveSecondaryNegotiator(int secondaryNegotiator)
        {
            this.page.RemoveSecondaryNegotiator(secondaryNegotiator);
        }

        [Then(@"Edit contact page should be displayed")]
        public void CheckEditContactPage()
        {
            Assert.True(this.page.IsEditContactFormPresent());
        }
    }
}
