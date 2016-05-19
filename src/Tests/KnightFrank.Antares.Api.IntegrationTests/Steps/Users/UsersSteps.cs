namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Resource;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Domain.User.QueryResults;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    using Xunit;

    [Binding]
    public class UsersSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/users";
        private readonly BaseTestClassFixture fixture;
        private readonly ScenarioContext scenarioContext;

        public UsersSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"All users have been deleted")]
        public void DeleteUsers()
        {
            this.fixture.DataContext.Users.RemoveRange(this.fixture.DataContext.Users.ToList());
        }

        [Given(@"User creates users in database with the following data")]
        public void CreateUsersInDb(Table table)
        {
            var country = new Country { IsoCode = "xx" };
            this.fixture.DataContext.Countries.Add(country);
            this.fixture.DataContext.SaveChanges();

            var business = new Business { CountryId = country.Id, Name = "Test Business 1" };
            this.fixture.DataContext.Businesses.Add(business);

            var department = new Department { CountryId = country.Id, Name = "Test Department 1" };
            this.fixture.DataContext.Departments.Add(department);

            var locale = new Locale { IsoCode = "xx" };
            this.fixture.DataContext.Locales.Add(locale);
            this.fixture.DataContext.SaveChanges();

            IEnumerable<User> users = table.CreateSet<User>().ToList();

            foreach (User user in users)
            {
                user.CountryId = country.Id;
                user.BusinessId = business.Id;
                user.DepartmentId = department.Id;
                user.LocaleId = locale.Id;
            }

            this.fixture.DataContext.Users.AddRange(users);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(users, "User List");
        }

        [When(@"User inputs (.*) query")]
        public void GetContactDetailsUsingId(string query)
        {
            string requestUrl = $"{ApiUrl}?partialName={query}";
            HttpResponseMessage response = this.fixture.SendGetRequest(requestUrl);

            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"User should get (.*) number of results returned")]
        public void CheckCorrectNumberOfResultsReturned(int matchCount)
        {
            string response = this.scenarioContext.GetResponseContent();
            var returnedUserList = JsonConvert.DeserializeObject<List<UsersQueryResult>>(response);
            returnedUserList.Count.ShouldBeEquivalentTo(matchCount);
        }

        [Then(@"User should get results in correct format")]
        public void CheckUserDetailsInCorrectFormat()
        {
            var userList = this.scenarioContext.Get<List<User>>("User List");

            string response = this.scenarioContext.GetResponseContent();
            var returnedUserList = JsonConvert.DeserializeObject<List<UsersQueryResult>>(response);

            Assert.All(returnedUserList, user =>
            {
                IEnumerable<User> matchingUsers = userList.Where(actual => actual.Id == user.Id
                                                                           && actual.Department.Name == user.Department);
                Assert.True(matchingUsers.Count() == 1);
            });
        }
    }
}
