namespace KnightFrank.Antares.UITests.Steps
{
    using System;

    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.UITests.Pages;

    using Objectivity.Test.Automation.Common;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class ViewPropertySteps
    {
        private readonly DriverContext driverContext;
        private readonly ScenarioContext scenarioContext;

        public ViewPropertySteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.driverContext = this.scenarioContext["DriverContext"] as DriverContext;
        }

        [Then(@"New property should be created with address details")]
        public void CheckIfPropertyCreated(Table table)
        {
            var address = table.CreateInstance<Address>();
            var page = new ViewPropertyPage(this.driverContext);

            Assert.Equal(address.Country.IsoCode, page.GetCountry());
            Assert.Equal(address.County, page.GetCounty());
            Assert.Equal(address.PropertyNumber, page.GetPropertyNumber());
            Assert.Equal(address.PropertyName, page.GetPropertyName());
            Assert.Equal(address.Line2, page.GetAddressLine2());
            Assert.Equal(address.Postcode, page.GetPostCode());
            Assert.Equal(address.City, page.GetCity());

            this.scenarioContext["ViewPropertyPage"] = page;
        }

        [When(@"User cliks add activites button on property details page")]
        public void ClickAddActivityButton()
        {
            this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").AddActivity();
        }

        [Then(@"Activity details are set on activity panel")]
        public void CheckActivityDetailsonActivityPanel(Table table)
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");

            //Assert.Equal(table.Rows[0]["Vendor"], page.Activity.GetActivityVendor());
            Assert.Equal(table.Rows[0]["Status"], page.Activity.GetActivityStatus());
        }

        [When(@"User selects save button on activity panel")]
        public void ClickSaveButtonOnActivityPanel()
        {
            this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage").Activity.SaveActivity();
        }

        [Then(@"Activity creation date is set to current date on property details page")]
        public void CheckifActivityDateCorrect()
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            Assert.Equal(DateTime.Now.ToString("dd-MM-yyyy"), page.GetActivityDate());
        }

        [Then(@"Activity details are set on property details page")]
        public void CheckActivityDetails(Table table)
        {
            var page = this.scenarioContext.Get<ViewPropertyPage>("ViewPropertyPage");
            Assert.Equal(table.Rows[0]["Vendor"], page.GetActivityVendor());
            Assert.Equal(table.Rows[0]["Status"], page.GetActivityStatus());
        }
    }
}
