using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KnightFrank.Antares.Api.IntegrationTests.Steps.AddressForm
{
    using System.Data.Entity;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class AddressFormSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/AddressForm";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public AddressFormSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Country code (.*) is present in DB")]
        public void GivenCountryCodeIsPresentInDb(string countryCode)
        {
            var country = new Country { Code = countryCode };
            this.fixture.DataContext.Country.Add(country);
            this.fixture.DataContext.SaveChanges();
        }

        [When(@"User retrieves address template for (.*) entity type and (.*) contry code")]
        public void WhenUserTryToRetrieveContactsDetailsForFollwoingData(string entityType, string countryCode)
        {
            string requestUrl = $"{ApiUrl}?entityType=" + entityType + "&countryCode=" + countryCode + "";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
