namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Domain.Enum.QueryResults;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class EnumsSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "api/enums";
        private readonly BaseTestClassFixture fixture;
        private readonly ScenarioContext scenarioContext;

        public EnumsSteps(BaseTestClassFixture fixture, ScenarioContext context)
        {
            this.fixture = fixture;

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.scenarioContext = context;
        }

        private EnumType EnumType { get; set; }
        private EnumTypeItem EnumTypeItem { get; set; }
        private EnumLocalised EnumLocalised { get; set; }

        [Given(@"There is EnumType")]
        public void GivenThereIsEnumType(Table table)
        {
            this.EnumType = table.CreateInstance<EnumType>();
            this.fixture.DataContext.EnumTypes.Add(this.EnumType);
            this.fixture.DataContext.SaveChanges();
        }

        [Given(@"There is EnumTypeItem")]
        public void GivenThereIsEnumTypeItem(Table table)
        {
            this.EnumTypeItem = table.CreateInstance<EnumTypeItem>();
            this.EnumTypeItem.EnumTypeId = this.EnumType.Id;
            this.fixture.DataContext.EnumTypeItems.Add(this.EnumTypeItem);
            this.fixture.DataContext.SaveChanges();
        }

        [Given(@"There is EnumLocalized for given EnumType and (.*) Locale")]
        public void GivenThereIsEnumLocalizedForGivenEnumTypeAndEnLocale(string localeIsoCode, Table table)
        {
            this.EnumLocalised = table.CreateInstance<EnumLocalised>();
            this.EnumLocalised.Locale = this.fixture.DataContext.Locales.FirstOrDefault(l => l.IsoCode == localeIsoCode.ToLower());
            this.EnumLocalised.EnumTypeItem = this.EnumTypeItem;

            this.fixture.DataContext.EnumLocaliseds.Add(this.EnumLocalised);
            this.fixture.DataContext.SaveChanges();
        }

        [Given(@"User gets EnumTypeItemId and EnumTypeItem code")]
        public void GetEnumTypeItemId(Table table)
        {
            var enums = new Dictionary<string, Guid>();

            foreach (TableRow row in table.Rows)
            {
                string enumTypeCode = row["enumTypeCode"];
                string enumTypeItemCode = row["enumTypeItemCode"];
                EnumTypeItem enumTypeItem =
                    this.fixture.DataContext.EnumTypeItems.SingleOrDefault(
                        i => i.EnumType.Code.Equals(enumTypeCode) && i.Code.Equals(enumTypeItemCode));

                enums.Add(enumTypeItemCode, enumTypeItem?.Id ?? Guid.NewGuid());
            }

            this.scenarioContext.Set(enums, "EnumDictionary");
        }

        [When(@"User retrieves EnumTypes by (.*) code")]
        public void WhenUserRetrievesEnumTypesByEntityTypeCode(string code)
        {
            string requestUrl = $"{ApiUrl}/{code}/items";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User retrieves Enums")]
        public void WhenUserRetrievesEnums()
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
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

        [Then(@"Result should get appropriate enums with enums type")]
        public void ThenResultShouldGetAppropriateEnumsWithEnumsType()
        {
            var dict = new Dictionary<string, ICollection<EnumItemResult>>();

            Dictionary<string, ICollection<EnumItemResult>> actualResult =
                JsonConvert.DeserializeObject<Dictionary<string, ICollection<EnumItemResult>>>(
                    this.scenarioContext.GetResponseContent()).OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            List<EnumItemResult> enumTypeList =
                this.fixture.DataContext.EnumTypes.Select(x => new EnumItemResult { Id = x.Id, Code = x.Code }).ToList();
            foreach (EnumItemResult enumType in enumTypeList)
            {
                List<EnumItemResult> list =
                    this.fixture.DataContext.EnumTypeItems.Where(x => x.EnumTypeId == enumType.Id)
                        .Select(a => new EnumItemResult
                        {
                            Id = a.Id,
                            Code = a.Code
                        }).ToList();
                dict.Add(char.ToLowerInvariant(enumType.Code[0]) + enumType.Code.Substring(1), list);
            }
            actualResult.ShouldBeEquivalentTo(dict);
        }
    }
}
