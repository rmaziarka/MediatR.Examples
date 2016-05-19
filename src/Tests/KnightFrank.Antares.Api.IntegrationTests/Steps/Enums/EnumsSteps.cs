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

        [When(@"User retrieves Enums")]
        public void WhenUserRetrievesEnums()
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
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
