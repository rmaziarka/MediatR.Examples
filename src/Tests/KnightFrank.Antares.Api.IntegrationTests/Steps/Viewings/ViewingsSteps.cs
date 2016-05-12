namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Viewings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Viewing.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;

    using Xunit;

    [Binding]
    public class ViewingsSteps : IClassFixture<BaseTestClassFixture>
    {
        private const string ApiUrl = "/api/viewings";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public ViewingsSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"User gets negotiator id from database")]
        public void GetNegotiatorId()
        {
            Guid negotiatorId = this.fixture.DataContext.Users.First().Id;
            this.scenarioContext.Set(negotiatorId, "NegotiatorId");
        }

        [When(@"User creates viewing using api")]
        public void CreateViewingUsingApi()
        {
            string requestUrl = $"{ApiUrl}";
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            List<Guid> attendeesIds =
                this.scenarioContext.Get<Requirement>("Requirement").Contacts.Select(contact => contact.Id).ToList();
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            var negotiatorId = this.scenarioContext.Get<Guid>("NegotiatorId");

            var details = new CreateViewingCommand
            {
                ActivityId = activityId,
                AttendeesIds = attendeesIds,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1),
                InvitationText = StringExtension.GenerateMaxAlphanumericString(4000),
                NegotiatorId = negotiatorId,
                RequirementId = requirementId
            };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, details);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User creates viewing with invalid (.*) using api")]
        public void CreateViewingWithInvalidDataUsingApi(string data)
        {
            string requestUrl = $"{ApiUrl}";
            Guid activityId = data.Equals("activity") ? Guid.NewGuid() : this.scenarioContext.Get<Activity>("Activity").Id;
            List<Guid> attendeesIds = data.Equals("attendee")
                ? new List<Guid> { Guid.NewGuid() }
                : this.scenarioContext.Get<Requirement>("Requirement").Contacts.Select(contact => contact.Id).ToList();
            Guid requirementId = data.Equals("requirement")
                ? Guid.NewGuid()
                : this.scenarioContext.Get<Requirement>("Requirement").Id;
            var negotiatorId = this.scenarioContext.Get<Guid>("NegotiatorId");

            var details = new CreateViewingCommand
            {
                ActivityId = activityId,
                AttendeesIds = attendeesIds,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1),
                InvitationText = StringExtension.GenerateMaxAlphanumericString(4000),
                PostViewingComment = StringExtension.GenerateMaxAlphanumericString(4000),
                NegotiatorId = negotiatorId,
                RequirementId = requirementId
            };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, details);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Given(@"User creates viewing in database")]
        public void CreateViewingInDatabase()
        {
            var requirement = this.scenarioContext.Get<Requirement>("Requirement");

            var viewing = new Viewing
            {
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1),
                InvitationText = StringExtension.GenerateMaxAlphanumericString(4000),
                Attendees = new List<Contact>
                {
                    new Contact
                    {
                        Id = requirement.Contacts[0].Id,
                        FirstName = requirement.Contacts[0].FirstName,
                        Surname = requirement.Contacts[0].Surname,
                        Title = requirement.Contacts[0].Title
                    }
                },
                RequirementId = requirement.Id,
                ActivityId = this.scenarioContext.Get<Activity>("Activity").Id,
                NegotiatorId = this.scenarioContext.Get<Guid>("NegotiatorId")
            };

            this.fixture.DataContext.Viewing.Add(viewing);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(viewing, "Viewing");
        }

        [When(@"User updates viewing")]
        public void UpdateViewing()
        {
            string requestUrl = $"{ApiUrl}";
            var viewing = this.scenarioContext.Get<Viewing>("Viewing");
            var details = new UpdateViewingCommand
            {
                Id = viewing.Id,
                AttendeesIds = new List<Guid>
                {
                    this.scenarioContext.Get<Requirement>("Requirement").Contacts[1].Id
                },
                PostViewingComment = StringExtension.GenerateMaxAlphanumericString(4000),
                InvitationText = StringExtension.GenerateMaxAlphanumericString(4000),
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2)
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, details);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [When(@"User updates viewing with invalid (.*) data")]
        public void UpdateViewingWithInvalidData(string data)
        {
            string requestUrl = $"{ApiUrl}";
            var viewing = this.scenarioContext.Get<Viewing>("Viewing");
            var details = new UpdateViewingCommand
            {
                Id = data.Equals("viewing") ? Guid.NewGuid() : viewing.Id,
                AttendeesIds = new List<Guid>
                {
                    data.Equals("attendee") ? Guid.NewGuid() : this.scenarioContext.Get<Requirement>("Requirement").Contacts[1].Id
                },
                PostViewingComment = StringExtension.GenerateMaxAlphanumericString(4000),
                InvitationText = StringExtension.GenerateMaxAlphanumericString(4000),
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow.AddHours(2)
            };

            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, details);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Viewing details should be the same as already added")]
        public void ThenViewingDetailsShouldBeTheSameAsAlreadyAdded()
        {
            var viewing = JsonConvert.DeserializeObject<Viewing>(this.scenarioContext.GetResponseContent());
            Viewing expectedViewing = this.fixture.DataContext.Viewing.Single(v => v.Id.Equals(viewing.Id));

            expectedViewing.ShouldBeEquivalentTo(viewing, opt => opt
                .Excluding(v => v.Negotiator)
                .Excluding(v => v.Activity)
                .Excluding(v => v.Requirement));
        }
    }
}
