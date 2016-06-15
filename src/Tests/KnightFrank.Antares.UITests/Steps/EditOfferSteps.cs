namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.UITests.Pages;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class EditOfferSteps
    {
        private const string Format = "dd-MM-yyyy";
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;
        private DateTime date = DateTime.UtcNow;
        private EditOfferPage page;

        public EditOfferSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;

            if (this.page == null)
            {
                this.page = new EditOfferPage(this.driverContext);
            }
        }

        [When(@"User navigates to edit offer page with id")]
        public void OpenEditActivityPageWithId()
        {
            Guid offerId = this.scenarioContext.Get<Offer>("Offer").Id;
            this.page = new EditOfferPage(this.driverContext).OpenEditOfferPageWithId(offerId.ToString());
        }

        [When(@"User fills in offer details on edit offer page")]
        public void FillOfferDetails(Table table)
        {
            var details = table.CreateInstance<OfferData>();

            if (this.scenarioContext.ContainsKey("Offer"))
            {
                string offerScenarioType = this.scenarioContext.Single(el => el.Key.Equals("Offer")).Value.GetType().Name;
                if (offerScenarioType.Equals(typeof(Offer).Name))
                {
                    details.OfferDate = this.scenarioContext.Get<Offer>("Offer").OfferDate.AddDays(-1).ToString(Format);
                }
                else if (offerScenarioType.Equals(typeof(OfferData).Name))
                {
                    DateTime offerDate = DateTime.ParseExact(this.scenarioContext.Get<OfferData>("Offer").OfferDate, Format,
                        CultureInfo.InvariantCulture);
                    details.OfferDate = offerDate.AddDays(-1).ToString(Format);
                }
            }
            else
            {
                details.OfferDate = this.date.ToString(Format);
            }

            details.ExchangeDate = this.date.AddDays(1).ToString(Format);
            details.CompletionDate = this.date.AddDays(2).ToString(Format);

            this.page.SelectStatus(details.Status)
                .SetOffer(details.Offer)
                .SetOfferDate(details.OfferDate)
                .SetSpecialConditions(details.SpecialConditions)
                .SetProposedExchangeDate(details.ExchangeDate)
                .SetProposedCompletionDate(details.CompletionDate);

            this.scenarioContext.Set(details, "Offer");
        }

        [When(@"User clicks save offer on edit offer page")]
        public void SaveOffer()
        {
            this.page.SaveOffer();
        }

        [When(@"User clicks activity details on edit offer page")]
        public void OpenActivityDetails()
        {
            this.page.OpenActivityPreview().WaitForSidePanelToShow();
        }

        [When(@"User fills in offer progress summary on edit offer page")]
        public void FillOfferProgressSummary(Table table)
        {
            var details = table.CreateInstance<OfferProgressSummary>();

            this.page.SelectMortgageStatus(details.MortgageStatus)
                .SelectMortgageSurveyStatus(details.MortgageSurveyStatus)
                .SelectAdditionalSurveyStatus(details.AdditionalSurveyStatus)
                .SelectSearchStatus(details.SearchStatus)
                .SelectEnquiries(details.Enquiries);

            if (details.ContractApproved)
            {
                this.page.ApproveContract();
            }
            else
            {
                this.page.DisapproveContract();
            }
        }

        [When(@"User fills in offer mortgage details on edit offer page")]
        public void FillOfferMortgageDetails(Table table)
        {
            var details = table.CreateInstance<OfferMortgageDetails>();
            details.MortgageSurveyDate = this.date.AddDays(4).ToString(Format);

            this.page.SetMortgageLoanToValue(details.MortgageLoanToValue).SetMortgageSurveyDate(details.MortgageSurveyDate);

            this.page.EditBroker()
                .WaitForSidePanelToShow()
                .ContactsList.WaitForContactsListToLoad()
                .SelectContact(details.Broker, details.BrokerCompany)
                .ApplyContact();
            this.page.WaitForSidePanelToHide();

            this.page.EditLender()
                .WaitForSidePanelToShow()
                .ContactsList.WaitForContactsListToLoad()
                .SelectContact(details.Lender, details.LenderCompany)
                .ApplyContact();
            this.page.WaitForSidePanelToHide();

            this.page.EditMortgageSurveyor()
                .WaitForSidePanelToShow()
                .ContactsList.WaitForContactsListToLoad()
                .SelectContact(details.Surveyor, details.SurveyorCompany)
                .ApplyContact();
            this.page.WaitForSidePanelToHide();
        }

        [When(@"User fills in offer additional details on edit offer page")]
        public void FillOfferAdditionalDetails(Table table)
        {
            var details = table.CreateInstance<OfferAdditional>();
            details.AdditionalSurveyDate = this.date.AddDays(3).ToString(Format);

            this.page.SetAdditionalSurveyDate(details.AdditionalSurveyDate).SetComment(details.Comment);

            this.page.EditAdditionalSurveyor()
                .WaitForSidePanelToShow()
                .ContactsList.WaitForContactsListToLoad()
                .SelectContact(details.AdditionalSurveyor, details.AdditionalSurveyorCompany)
                .ApplyContact();
            this.page.WaitForSidePanelToHide();
        }

        [Then(@"Following company contacts should be displayed on edit offer page")]
        public void CheckContacts(Table table)
        {
            var expectedUsers = table.CreateInstance<OfferMortgageDetails>();
            var expectedAdditionalUsers = table.CreateInstance<OfferAdditional>();

            List<string> currentBroker = this.page.Broker;
            List<string> currentLender = this.page.Lender;
            List<string> currentMortgageSurveyor = this.page.MortgageSurveyor;
            List<string> currentAdditionalSurveyor = this.page.AdditionalSurveyor;

            Verify.That(this.driverContext,
                () => Assert.Equal(expectedUsers.Broker, currentBroker.First()),
                () => Assert.Equal(expectedUsers.BrokerCompany, currentBroker.Last()),
                () => Assert.Equal(expectedUsers.Lender, currentLender.First()),
                () => Assert.Equal(expectedUsers.LenderCompany, currentLender.Last()),
                () => Assert.Equal(expectedUsers.Surveyor, currentMortgageSurveyor.First()),
                () => Assert.Equal(expectedUsers.SurveyorCompany, currentMortgageSurveyor.Last()),
                () => Assert.Equal(expectedAdditionalUsers.AdditionalSurveyor, currentAdditionalSurveyor.First()),
                () => Assert.Equal(expectedAdditionalUsers.AdditionalSurveyorCompany, currentAdditionalSurveyor.Last()));
        }
    }
}
