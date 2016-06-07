namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.UITests.Pages;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class EditOfferSteps
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DriverContext driverContext;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ScenarioContext scenarioContext;
        private EditOfferPage page;
        private const string Format = "dd-MM-yyyy";

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

            details.OfferDate = this.scenarioContext.ContainsKey("Offer")
                ? this.scenarioContext.Get<Offer>("Offer").OfferDate.AddDays(-1).ToString(Format)
                : DateTime.UtcNow.ToString(Format);
            details.ExchangeDate = DateTime.UtcNow.AddDays(1).ToString(Format);
            details.CompletionDate = DateTime.UtcNow.AddDays(2).ToString(Format);

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
    }
}
