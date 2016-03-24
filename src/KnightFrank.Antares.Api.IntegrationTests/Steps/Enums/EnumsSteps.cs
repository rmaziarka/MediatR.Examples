namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Domain.Enum.QueryResults;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class EnumsSteps : IClassFixture<BaseTestClassFixture>
    {
        private readonly BaseTestClassFixture fixture;
        private const string ApiUrl = "api/enums";
        private readonly ScenarioContext scenarioContext;
        private EnumType EnumType { get; set; }
        private EnumTypeItem EnumTypeItem { get; set; }
        private EnumLocalised EnumLocalised { get; set; }

        public EnumsSteps(BaseTestClassFixture fixture, ScenarioContext context)
        {
            this.fixture = fixture;

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }                      

            this.scenarioContext = context;
        }

        [When(@"User retrieves EnumTypes by (.*) code")]
        public void WhenUserRetrievesEnumTypesByEntityTypeCode(string code)
        {
            string requestUrl = $"{ApiUrl}/" + code + "/items";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
        }
        
        [Given(@"There is EnumType")]
        public void GivenThereIsEnumType(Table table)
        {
            this.EnumType = table.CreateInstance<EnumType>();
            this.fixture.DataContext.EnumType.Add(this.EnumType);
            this.fixture.DataContext.SaveChanges();
        }

        [Given(@"There is EnumTypeItem")]
        public void GivenThereIsEnumTypeItem(Table table)
        {
            this.EnumTypeItem = table.CreateInstance<EnumTypeItem>();
            this.EnumTypeItem.EnumTypeId = this.EnumType.Id;
            this.fixture.DataContext.EnumTypeItem.Add(this.EnumTypeItem);
            this.fixture.DataContext.SaveChanges();
        }

        [Given(@"There is EnumLocalized for given EnumType and (.*) Locale")]
        public void GivenThereIsEnumLocalizedForGivenEnumTypeAndEnLocale(string localeIsoCode, Table table)
        {
            this.EnumLocalised = table.CreateInstance<EnumLocalised>();
            this.EnumLocalised.Locale = this.fixture.DataContext.Locale.FirstOrDefault(l => l.IsoCode == localeIsoCode.ToLower());
            this.EnumLocalised.EnumTypeItem = this.EnumTypeItem;

            this.fixture.DataContext.EnumLocalised.Add(this.EnumLocalised);
            this.fixture.DataContext.SaveChanges();
        }

        [Then(@"Result should contain single element")]
        public void ThenResultShouldContainSingleElement()
        {
            var result = JsonConvert.DeserializeObject<EnumQueryResult>(this.scenarioContext.GetResponseContent());

            result.Items.Should().ContainSingle();
        }

        [Then(@"Single element has Id being set")]
        public void ThenSingleElementHasIdBeingSet()
        {
            var result = JsonConvert.DeserializeObject<EnumQueryResult>(this.scenarioContext.GetResponseContent());

            result.Items.Should().Contain(x => x.Id != null);
        }

        [Then(@"Single element should be equal to")]
        public void ThenResultShouldContain(Table table)
        {
            IEnumerable<EnumQueryItemResult> expectedResult = table.CreateSet<EnumQueryItemResult>();

            var result = JsonConvert.DeserializeObject<EnumQueryResult>(this.scenarioContext.GetResponseContent());

            expectedResult.Select(er => er.Value).ShouldBeEquivalentTo(result.Items.Select(r => r.Value));
        }
    }
}
