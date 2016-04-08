namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.UITests.Pages;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class CreateOwnershipSteps
    {
        private readonly ScenarioContext scenarioContext;

        public CreateOwnershipSteps(ScenarioContext scenarioContext)
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
            var details = table.CreateInstance<OwnershipDetails>();

            if (Convert.ToBoolean(details.Current))
            {
                page.Ownership.SetCurrentOwnership()
                    .SelectOwnershipType(details.Type);
                if (!string.IsNullOrEmpty(details.PurchasePrice))
                {
                    page.Ownership.SetPurchasePrice(details.PurchasePrice);
                }
                if (!string.IsNullOrEmpty(details.PurchaseDate))
                {
                    page.Ownership.SetPurchaseDate(details.PurchaseDate);
                }
            }
            else
            {
                page.Ownership.SetOwnership()
                    .SelectOwnershipType(details.Type);
                if (!string.IsNullOrEmpty(details.PurchasePrice))
                {
                    page.Ownership.SetPurchasePrice(details.PurchasePrice);
                }
                if (!string.IsNullOrEmpty(details.SellPrice))
                {
                    page.Ownership.SetSellPrice(details.SellPrice);
                }
                if (!string.IsNullOrEmpty(details.PurchaseDate))
                {
                    page.Ownership.SetPurchaseDate(details.PurchaseDate);
                }
                if (!string.IsNullOrEmpty(details.SellDate))
                {
                    page.Ownership.SetSellDate(details.SellDate);
                }
            }
            page.Ownership.SaveOwnership().WaitForOwnershipPanelToHide();
        }
    }

    internal class OwnershipDetails
    {
        public int Position { get; set; }

        public string ContactName { get; set; }

        public string ContactSurname { get; set; }

        public string Type { get; set; }

        public bool Current { get; set; }

        public string PurchaseDate { get; set; }

        public string SellDate { get; set; }

        public string PurchasePrice { get; set; }

        public string SellPrice { get; set; }
    }
}
