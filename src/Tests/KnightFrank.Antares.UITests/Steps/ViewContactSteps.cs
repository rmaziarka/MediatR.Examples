namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FluentAssertions;

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
        private readonly ScenarioContext scenarioContext;
        private ViewContactPage page;

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

        [When(@"User navigates to view contact page with id")]
        public void OpenViewContactPage()
        {
            List<Guid> contactsIds = this.scenarioContext.Get<List<Contact>>("ContactsList").Select(c => c.Id).ToList();

            foreach (Guid element in contactsIds)
            {
                this.page = new ViewContactPage(this.driverContext).OpenViewContactPageWithId(element.ToString());
                this.page.IsViewContactFormPresent();
            }
        }

        [When(@"User clicks edit button on view contact page")]
        public void OpenEditContactPage()
        {
            this.page.OpenEditContactPage();
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

        [Then(@"Primary negotiator is (.*) on view contact page")]
        public void CheckPrimaryNegotiator(string negotiator)
        {
            Verify.That(this.driverContext, () => Assert.Equal(negotiator, this.page.PrimaryNegotiator));
        }

        [Then(@"Secondary negotiators on view contact page are the same as following")]
        public void CheckSecondaryNegotiator(Table table)
        {
            List<string> negotiators = table.CreateSet<Negotiator>().Select(n => n.Name).ToList();
            negotiators.Should().Equal(this.page.SecondaryNegotiators);
        }
    }
}
