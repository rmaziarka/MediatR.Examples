namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.UITests.Pages;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class OwnershipDetailsSteps
    {
        private readonly ScenarioContext scenarioContext;

        public OwnershipDetailsSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
        }

        [When(@"User fills in ownership details on ownership details page")]
        public void FillInOwnershipDetails(Table table)
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            var details = table.CreateInstance<Ownership>();
     
            if (Convert.ToBoolean(details.Current))
            {
                page.Ownership.SetCurrentOwnership()
                    .SelectOwnershipType(details.Type)
                    .SetBuyPrice(details.BuyPrice);
                if (!details.PurchaseDate.Equals(string.Empty))
                {
                    page.Ownership.SetPurchaseDate(details.PurchaseDate);
                }
            }
            else
            {
                page.Ownership.SetOwnership()
                    .SelectOwnershipType(details.Type)
                    .SetBuyPrice(details.BuyPrice)
                    .SetSellPrice(details.SellPrice);
                if (!details.PurchaseDate.Equals(string.Empty))
                {
                    page.Ownership.SetPurchaseDate(details.PurchaseDate);
                }
                if (!details.SellDate.Equals(string.Empty))
                {
                    page.Ownership.SetSellDate(details.SellDate);
                }
            }
            page.Ownership.SaveOwnership().WaitForOwnershipPanelToHide();
        }
    }

    internal class Ownership
    {
        public Contact Contact { get; set; } 

        public string Type { get; set; }

        public bool Current { get; set; }

        public string PurchaseDate { get; set; }

        public string SellDate { get; set; }

        public string BuyPrice { get; set; }

        public string SellPrice { get; set; }
    }
}
