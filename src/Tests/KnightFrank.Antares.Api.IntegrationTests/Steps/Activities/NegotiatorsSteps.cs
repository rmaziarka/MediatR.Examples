namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Activities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Domain.Activity.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;

    [Binding]
    public class NegotiatorsSteps
    {
        private const string ApiUrl = "/api/activities";
        private readonly DateTime date = DateTime.UtcNow;
        private readonly BaseTestClassFixture fixture;
        private readonly ScenarioContext scenarioContext;

        private Activity activity;
        private ActivityUser activityUser;

        private List<Guid> secondaryNegotiatorsList;
        private List<User> secondaryNegotiatorUsersList;

        private List<UpdateActivityDepartment> updateActivityDepartments;

        private UpdateActivityUser updateActivityUser;

        public NegotiatorsSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Lead negotiator with ActiveDirectoryLogin (.*) and today plus (.*) next call date exists in database")]
        public void GivenLeadNegotiatorWithActiveDirectoryLoginExistsInDatabase(string activeDirectoryLogin, double noOfDays)
        {
            User user = this.fixture.DataContext.Users.SingleOrDefault(x => x.ActiveDirectoryLogin.Equals(activeDirectoryLogin));
            Guid departmentTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["Managing"];
            this.updateActivityUser = new UpdateActivityUser
            {
                UserId = user?.Id ?? Guid.NewGuid(),
                CallDate = this.date.AddDays(noOfDays)
            };

            this.updateActivityDepartments = new List<UpdateActivityDepartment>
            {
                new UpdateActivityDepartment
                {
                    DepartmentId = user?.DepartmentId ?? Guid.Empty,
                    DepartmentTypeId = departmentTypeId
                }
            };
        }

        [Given(@"Following secondary negotiators exists in database")]
        public void GivenFollowingSecondaryNegotiatorsExistsInDatabase(Table table)
        {
            this.secondaryNegotiatorUsersList =
                table.Rows.Select(row => row["ActiveDirectoryLogin"])
                     .Select(login => this.fixture.DataContext.Users.Single(x => x.ActiveDirectoryLogin.Equals(login)))
                     .ToList();

            this.secondaryNegotiatorsList = this.secondaryNegotiatorUsersList.Select(x => x.Id).ToList();

            Guid departmentTypeId = this.scenarioContext.Get<Dictionary<string, Guid>>("EnumDictionary")["Standard"];

            foreach (User secondaryNegotiator in this.secondaryNegotiatorUsersList)
            {
                this.updateActivityDepartments.Add(new UpdateActivityDepartment
                {
                    DepartmentId = secondaryNegotiator.DepartmentId,
                    DepartmentTypeId = departmentTypeId
                });
            }
        }

        [When(@"User updates activity with defined negotiators")]
        public void UpdateActivityNegotiators()
        {
            string requestUrl = $"{ApiUrl}";

            this.secondaryNegotiatorsList = this.secondaryNegotiatorsList ?? new List<Guid>();
            this.activity = this.scenarioContext.Get<Activity>("Activity");

            var updateActivityCommand = new UpdateActivityCommand
            {
                Id = this.activity.Id,
                ActivityTypeId = this.activity.ActivityTypeId,
                ActivityStatusId = this.activity.ActivityStatusId,
                LeadNegotiator = this.updateActivityUser,
                SecondaryNegotiators =
                    this.secondaryNegotiatorsList.Select(
                        n => new UpdateActivityUser { UserId = n, CallDate = this.date.AddDays(10) }).ToList(),
                Departments = this.updateActivityDepartments
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, updateActivityCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User updates last call date by adding (.*) days for (.*) user")]
        public void UpdateLastCallDate(string noOfDaysToAdd, string user)
        {
            this.activity = this.scenarioContext.Get<Activity>("Activity");
            string requestUrl = $"{ApiUrl}/{this.activity.Id}/negotiators";
            this.activityUser = this.activity.ActivityUsers.First();

            Guid activityUserId = user.Equals("valid") ? this.activityUser.Id : Guid.NewGuid();

            DateTime? callDate = noOfDaysToAdd.Equals("null")
                ? (DateTime?)null
                : this.date.AddDays(double.Parse(noOfDaysToAdd));

            var updateActivityUserCommand = new UpdateActivityUserCommand
            {
                CallDate = callDate,
                ActivityId = this.activity.Id,
                Id = activityUserId
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, updateActivityUserCommand);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Last call date should be updated in data base")]
        public void ThenLastCallDateShouldBeUpdatedInDataBase()
        {
            var user = JsonConvert.DeserializeObject<ActivityUser>(this.scenarioContext.GetResponseContent());
            ActivityUser updatedActivityUser = this.fixture.DataContext.ActivityUsers.Single(x => x.Id.Equals(this.activityUser.Id));

            updatedActivityUser.ShouldBeEquivalentTo(user, options => options
                .Excluding(x => x.Activity)
                .Excluding(x => x.User)
                .Excluding(x => x.UserType));
        }
    }
}
