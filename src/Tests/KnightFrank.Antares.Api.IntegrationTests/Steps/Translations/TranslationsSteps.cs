namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Translations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Property.Characteristics;
    using KnightFrank.Antares.Dal.Model.Resource;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class TranslationsSteps
    {
        private const string ApiUrl = "/api/translations/resources";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public TranslationsSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"There is a Locale for (.*) country code")]
        public void GivenThereIsALocaleForCountryCode(string isoCode)
        {
            var locale = new Locale { IsoCode = isoCode };
            this.fixture.DataContext.Locales.Add(locale);
            this.fixture.DataContext.SaveChanges();
            this.scenarioContext.Set(locale.Id, "LocaleId");
        }

        [Given(@"User creates following translations for characteristics in database")]
        public void GivenIHaveEnteredFollowedTranslationsForCharacteristics(Table table)
        {
            var localeId = this.scenarioContext.Get<Guid>("LocaleId");
            IEnumerable<TranslatedObject> translatedCharacteristic = table.CreateSet<TranslatedObject>();
            var list = new List<CharacteristicLocalised>();
            foreach (
                CharacteristicLocalised characteristicLocalized in
                    translatedCharacteristic.Select(translation => new CharacteristicLocalised
                    {
                        LocaleId = localeId,
                        Value = translation.TranslateValue,
                        Characteristic = this.fixture.DataContext.Characteristics.Single(x => x.Code.Equals(translation.Code))
                    }))
            {
                this.fixture.DataContext.CharacteristicLocaliseds.Add(characteristicLocalized);
                list.Add(characteristicLocalized);
            }
            this.fixture.DataContext.SaveChanges();
            this.scenarioContext.Set(list, "CharacteristicLocalised");
        }

        [Given(@"User creates following translations for countries in database")]
        public void GivenIHaveEnteredFollowedTranslationsForCountries(Table table)
        {
            var localeId = this.scenarioContext.Get<Guid>("LocaleId");
            IEnumerable<TranslatedObject> translatedCountries = table.CreateSet<TranslatedObject>();
            var list = new List<CountryLocalised>();

            foreach (CountryLocalised countryLocalized in translatedCountries.Select(translation => new CountryLocalised
            {
                LocaleId = localeId,
                Value = translation.TranslateValue,
                Country = this.fixture.DataContext.Countries.Single(x => x.IsoCode.Equals(translation.Code))
            }))
            {
                this.fixture.DataContext.CountryLocaliseds.Add(countryLocalized);
                list.Add(countryLocalized);
            }
            this.fixture.DataContext.SaveChanges();
            this.scenarioContext.Set(list, "CountryLocalised");
        }

        [When(@"User retrieves translations for (.*) isocode")]
        public void WhenUserRetrievesTranslationsFor_Isocode(string isoCode)
        {
            string requestUrl = $"{ApiUrl}/{isoCode}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Translations are as expected")]
        public void ThenTranslationsAreAsExpected()
        {
            var characteristicLocalised = this.scenarioContext.Get<List<CharacteristicLocalised>>("CharacteristicLocalised");
            var localizedCountries = this.scenarioContext.Get<List<CountryLocalised>>("CountryLocalised");

            Dictionary<Guid, string> dict = localizedCountries.ToDictionary(country => country.Country.Id, country => country.Value);
            foreach (CharacteristicLocalised characteristic in characteristicLocalised)
            {
                dict.Add(characteristic.Characteristic.Id, characteristic.Value);
            }

            var actualResponse = JsonConvert.DeserializeObject<Dictionary<Guid, string>>(this.scenarioContext.GetResponseContent());
            actualResponse.ShouldBeEquivalentTo(dict);
        }

        internal class TranslatedObject
        {
            public string Code { get; set; }
            public string TranslateValue { get; set; }
        }
    }
}
