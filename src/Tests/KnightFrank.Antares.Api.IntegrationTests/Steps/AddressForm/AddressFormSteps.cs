namespace KnightFrank.Antares.Api.IntegrationTests.Steps.AddressForm
{
    using System;
    using System.Linq;
    using System.Net.Http;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Address;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Resource;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class AddressFormSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/AddressForms";
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

        private AddressForm AddressForm { get; set; }

        [Given(@"Country code (.*) is present in DB")]
        public void GivenCountryCodeIsPresentInDb(string countryCode)
        {
            var country = new Country { IsoCode = countryCode };
            this.fixture.DataContext.Countries.Add(country);
            this.fixture.DataContext.SaveChanges();
        }

        [Given(@"There is an AddressForm for (.*) country code")]
        public void GivenThereIsAddressFormForUk(string countryIsoCode)
        {
            if (this.fixture.DataContext.Countries.FirstOrDefault(c => c.IsoCode == countryIsoCode) == null)
            {
                this.fixture.DataContext.Countries.Add(new Country { IsoCode = countryIsoCode });
                this.fixture.DataContext.SaveChanges();
            }

            Country country = this.fixture.DataContext.Countries.FirstOrDefault(c => c.IsoCode == countryIsoCode);

            this.AddressForm = new AddressForm { Country = country };

            this.fixture.DataContext.AddressForms.Add(this.AddressForm);
            this.fixture.DataContext.SaveChanges();
        }

        [Given(@"There exists AddressForm for (.*) EnumTypeItem")]
        public void GivenThereExistsAddressFormForPropertyEnumTypeItem(string enumTypeItemCode)
        {
            EnumTypeItem enumTypeItem = this.fixture.DataContext.EnumTypeItems.FirstOrDefault(x => x.Code == enumTypeItemCode);

            var addressFormEntityType = new AddressFormEntityType { EnumTypeItem = enumTypeItem, AddressForm = this.AddressForm };

            this.fixture.DataContext.AddressFormEntityTypes.Add(addressFormEntityType);
            this.fixture.DataContext.SaveChanges();
        }

        [When(@"User retrieves address template for (.*) entity type and (.*) contry code")]
        public void WhenUserTryToRetrieveContactsDetailsForFollowingData(string enumTypeItem, string countryCode)
        {
            string requestUrl = $"{ApiUrl}?entityType=" + enumTypeItem + "&countryCode=" + countryCode + "";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
