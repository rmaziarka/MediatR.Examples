namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.UITests.Pages;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class CreateChainTransactionSteps
    {
        private readonly DriverContext driverContext;
        private readonly CreateChainTransactionPage page;
        private readonly ScenarioContext scenarioContext;

        public CreateChainTransactionSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new ViewOfferPage(this.driverContext).ChainTransaction;
            }
        }

        [When(@"User fills in chain transaction details on view offer page")]
        public void SelectAgent(Table table)
        {
            var details = table.CreateInstance<ChainTransactionData>();
            if (bool.Parse(details.EndOfChain))
            {
                this.page.SelectEndOfChain();
            }
            this.page.SetVendor(details.Vendor);
        }

        [When(@"User selects property in chain transaction on view offer page")]
        public void SelectProperty()
        {
            Guid propertyId = this.scenarioContext.Get<Property>("Property").Id;
            this.page.AddProperty();
            this.page.PropertiesList.WaitForPropertiesListToLoad()
                .SelectProperty(propertyId.ToString())
                .ApplyProperty();
        }

        [When(@"User selects solicitor in chain transaction on view offer page")]
        public void SelectSolicitor(Table table)
        {
            var details = table.CreateInstance<ChainTransactionData>();
            this.page.AddSolicitor();
            this.page.ContactsList.WaitForContactsListToLoad()
                .SelectContact(details.Solicitor, details.SolicitorCompany)
                .ApplyContact();
        }

        [When(@"User selects knight frank agent in chain transaction on view offer page")]
        public void SelectKnightFrankAgent(Table table)
        {
            var details = table.CreateInstance<ChainTransactionData>();
            this.page.SelectKnightFrankAgent().SetKnightFrankAgent(details.KnightFrankAgent);
        }

        [When(@"User selects 3rd party agent in chain transaction on view offer page")]
        public void SelectThirdPartyAgent(Table table)
        {
            var details = table.CreateInstance<ChainTransactionData>();
            this.page.SelectOtherAgent().AddOtherAgent();
            this.page.ContactsList.WaitForContactsListToLoad()
                .SelectContact(details.OtherAgent, details.OtherAgentCompany)
                .ApplyContact();
        }

        [When(@"User selects progress details in chain transaction on view offer page")]
        public void SelectProgressDetails(Table table)
        {
            var details = table.CreateInstance<ChainTransactionData>();
            this.page.SelectMortgage(details.Mortgage)
                .SelectSurvey(details.Survey)
                .SetSurveyDate(DateTime.UtcNow.ToString("dd-MM-yyyy"))
                .SelectSearches(details.Searches)
                .SelectEnquiries(details.Enquiries)
                .SelectContractAgreed(details.ContractAgreed);
        }

        [When(@"User clicks save chain transaction button on view offer page")]
        public void ClickSaveButton()
        {
            this.page.SaveChainTransaction().WaitForSidePanelToHide();
        }

        [Then(@"Chain solicitor details on view offer page are same as the following")]
        public void CheckChainSolicitor(Table table)
        {
            var details = table.CreateInstance<ChainTransactionData>();

            Verify.That(this.driverContext,
                () => Assert.Equal(details.Solicitor, this.page.Solicitor.First()),
                () => Assert.Equal(details.SolicitorCompany, this.page.Solicitor.Last()));
        }

        [Then(@"Chain knight frank agent details on view offer page are same as the following")]
        public void CheckChainKnightFrankAgent(Table table)
        {
            var details = table.CreateInstance<ChainTransactionData>();

            Verify.That(this.driverContext, () => Assert.Equal(details.KnightFrankAgent, this.page.KnightFrankAgent));
        }

        [Then(@"Chain 3rd party agent details on view offer page are same as the following")]
        public void CheckChainAgent(Table table)
        {
            var details = table.CreateInstance<ChainTransactionData>();

            Verify.That(this.driverContext, 
                () => Assert.Equal(details.OtherAgent, this.page.OtherAgent.First()),
                () => Assert.Equal(details.OtherAgentCompany, this.page.OtherAgent.Last()));
        }

        [Then(@"Chain property details on view offer page are same as the following")]
        public void CheckChainProperty(Table table)
        {
            var details = table.CreateInstance<ChainTransactionData>();

            Verify.That(this.driverContext, () => Assert.Equal(details.Property, this.page.GetProperty()));
        }
    }
}
