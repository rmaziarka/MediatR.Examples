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
    public class ViewContactSteps
    {
        private readonly DriverContext driverContext;
        private readonly ViewContactPage page;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;

        public ViewContactSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new ViewContactPage(this.driverContext);
            }
        }

        [Then(@"Contact details on view contact page are same as the following")]
        public void CheckContactDetails(Table table)
        {
            var details = table.CreateInstance<Contact>();
            string name = details.Title + " " + details.FirstName + " " + details.LastName;

            Verify.That(this.driverContext, () => Assert.Equal(name, this.page.Name));
        }

        [Then(@"Mailings salutations details on view contact page are same as the following")]
        public void CheckMailingsSalutations(Table table)
        {
            var details = table.CreateInstance<Salutation>();

            Verify.That(this.driverContext,
                () => Assert.Equal(details.DefaultMailingSalutation, this.page.DefaultMailingSalutation),
                () => Assert.Equal(details.MailingFormalSalutation, this.page.MailingFormalSalutation),
                () => Assert.Equal(details.MailingSemiformalSalutation, this.page.MailingSemiformalSalutation),
                () => Assert.Equal(details.MailingInformalSalutation, this.page.MailingInformalSalutation),
                () => Assert.Equal(details.MailingPersonalSalutation, this.page.MailingPersonalSalutation),
                () => Assert.Equal(details.MailingEnvelopeSalutation, this.page.MailingEnvelopeSalutation));
        }

        [Then(@"View contact page should be displayed")]
        public void CheckViewContactPage()
        {
            Assert.True(this.page.IsViewContactFormPresent());
        }

        [When(@"User navigates to Edit Contact page")]
        public void OpenEditContactPage()
        {
            this.page.OpenEditContactPage();
        }
    }
}
