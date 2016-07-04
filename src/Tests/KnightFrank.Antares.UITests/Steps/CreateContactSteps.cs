namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

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
        [When(@"User navigates to create contact page")]
        public void OpenCreateContactPage()
        {
            this.page = new CreateContactPage(this.driverContext).OpenCreateContactPage();
        }

        [When(@"User fills in contact details on create contact page")]
        public void CreateContact(Table table)
        {
            var contact = table.CreateInstance<Contact>();

            this.page.SetFirstName(contact.FirstName)
                .SetLastName(contact.LastName)
                .SetTitle(contact.Title);
        }

        [When(@"User clicks save contact button on create contact page")]
        public void SaveContact()
        {
            this.page.SaveContact();
        }

        [Then(@"Contact form on create contact page should be displayed")]
        public void CheckIfContactFormPresent()
        {
            Assert.True(new CreateContactPage(this.driverContext).IsContactFormPresent());
        }

        [Then(@"Check Mailings Salutations")]
        public void CheckSemiMailingsSalutations(Table table)
        {
            var salutations = table.CreateInstance<Contact>();

            Verify.That(this.driverContext,
                () => Assert.Equal(salutations.MailingSemiformalSalutation, this.page.SemiformalMailingSalutation),
                () => Assert.Equal(salutations.MailingFormalSalutation, this.page.FormalMailingSalutation),
                () => Assert.Equal(salutations.MailingInformalSalutation, this.page.InformalMailingSalutation),
                () => Assert.Equal(salutations.MailingPersonalSalutation, this.page.PersonalMailingSalutation),
                () => Assert.Equal(salutations.MailingEnvelopeSalutation, this.page.EnvelopeMailingSalutation));
        }
    }
}
