namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.UITests.Extensions;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;
    using Objectivity.Test.Automation.Common.Extensions;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class CreateContactSteps
    {
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;
        private CreateContactPage page;

        public CreateContactSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new CreateContactPage(this.driverContext);
            }
        }

        [Given(@"User navigates to create contact page")]
        public void OpenCreateContactPage()
        {
            this.page = new CreateContactPage(this.driverContext).OpenCreateContactPage();
        }

        [When(@"User fills in contact details on create contact page")]
        [Given(@"User fills in contact details on create contact page")]
        public void CreateContact(Table table)
        {
            var contact = table.CreateInstance<Contact>();

            this.page.SetTitle(contact.Title)
                .SetFirstName(contact.FirstName)
                .SetLastName(contact.LastName);
        }

        [When(@"User clicks save contact button on create contact page")]
        public void SaveContact()
        {
            this.page.SaveContact();
        }

        [Then(@"New contact should be created")]
        public void CheckIfContactCreated()
        {
            //TODO implement check if contact was created
        }

        [Then(@"Contact form on create contact page should be displayed")]
        public void CheckIfContactFormPresent()
        {
            Assert.True(new CreateContactPage(this.driverContext).IsContactFormPresent());
        }

        [Then(@"User is taken to the contact add page")]
        public void UserIsTakenToTheContactAddPage()
        {
            Assert.True(new CreateContactPage(this.driverContext).CheckIfContactAddPage());
        }

        [When(@"User chooses '(.*)' as Mailings Use Salutation")]
        public void UserChoosesMailingsUseSalutation(string salutation)
        {
            this.page.SetMailingsUseSalutation(salutation);
        }

        [When(@"User chooses '(.*)' as Events Use Salutation")]
        public void UserChoosesEventsUseSalutation(string salutation)
        {
            this.page.SetEventsUseSalutation(salutation);
        }

        [Then(@"Check Mailings Salutations")]
        public void CheckSemiMailingsSalutations(Table table)
        {
            var salutations = table.CreateInstance<Contact>();

            Assert.True(string.Equals(salutations.MailingSemiformalSalutation, this.page.GetSemiformalMailingSalutation()));
            Assert.True(string.Equals(salutations.MailingFormalSalutation, this.page.GetFormalMailingSalutation()));
            Assert.True(string.Equals(salutations.MailingInformalSalutation, this.page.GetInformalMailingSalutation()));
            Assert.True(string.Equals(salutations.MailingPersonalSalutation, this.page.GetPersonalMailingSalutation()));
            Assert.True(string.Equals(salutations.MailingEnvelopeSalutation, this.page.GetEnvelopeMailingSalutation()));
        }
    }
}
