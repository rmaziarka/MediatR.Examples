namespace KnightFrank.Antares.UITests.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.UITests.Pages;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class OwnershipDetailsSteps
    {
        //        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;

        public OwnershipDetailsSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;

            //            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [When(@"User fills in ownership details on ownership details page")]
        public void FillInOwnershipDetails(Table table)
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            var details = table.CreateInstance<Ownership>();
            bool current = Convert.ToBoolean(table.Rows[0]["Current"]);

            if (current)
            {
                page.Ownership.SetCurrentOwnership()
                    .SelectOwnershipType(table.Rows[0]["Type"])
                    .SetBuyingPrice(details.BuyPrice.ToString());
                if (details.PurchaseDate != null)
                {
                    page.Ownership.SetPurchasingDate(details.PurchaseDate.Value.ToString("yyyy-MM-dd"));
                }
            }
            else
            {
                page.Ownership.SelectOwnershipType(table.Rows[0]["Type"])
                    .SetBuyingPrice(details.BuyPrice.ToString())
                    .SetSellingPrice(details.SellPrice.ToString());
                if (details.PurchaseDate != null)
                {
                    page.Ownership.SetPurchasingDate(details.PurchaseDate.Value.ToString("yyyy-MM-dd"));
                }
                if (details.SellDate != null)
                {
                    page.Ownership.SetSellingDate(details.SellDate.Value.ToString("yyyy-MM-dd"));
                }
            }
            page.Ownership.SaveOwnership().WaitForOwnershipPanelToHide();
        }

        [Then(@"Following contacts should be visible on ownership details page")]
        public void CheckContactsInOwnershipDetails(Table table)
        {
            List<string> expectedContacts = table.CreateSet<Contact>().Select(c => c.FirstName + " " + c.Surname).ToList();
            List<string> contacts = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").Ownership.Contacts;

            Assert.Equal(expectedContacts, contacts);
        }

        [Then(@"Ownership details should contain following data on ownership details page")]
        public void CheckOwnershipDetails(Table table)
        {
            //            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            //            List<string> expectedContacts = table.Rows[0]["Contacts"].Split(';').ToList();
            //            string expectedType = table.Rows[0]["Type"];
            //            string expectedPurchaseDat = table.Rows[0]["PurchaseDate"];
            //            string expectedSellDate = table.Rows[0]["SellDate"];
            //            string expectedBuyPrice = table.Rows[0]["BuyPrice"];
            //            string expectedSellPrice = table.Rows[0]["SellPrice"];
        }
    }
}
