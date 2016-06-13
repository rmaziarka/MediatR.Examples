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
        private readonly DateTime date = DateTime.UtcNow;
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

        [Given(@"Viewing exists in database")]
        public void CreateViewingInDatabase()
        {
            var requirement = this.scenarioContext.Get<Requirement>("Requirement");

            var viewing = new Viewing
            {
                StartDate = this.date,
                EndDate = this.date.AddHours(1),
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
                NegotiatorId = this.fixture.DataContext.Users.First().Id
            };

            this.fixture.DataContext.Viewing.Add(viewing);
            this.fixture.DataContext.SaveChanges();

            this.scenarioContext.Set(viewing, "Viewing");
        }

        [When(@"User creates viewing using api")]
        public void CreateViewingUsingApi()
        {
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            List<Guid> attendeesIds =
                this.scenarioContext.Get<Requirement>("Requirement").Contacts.Select(contact => contact.Id).ToList();
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            Guid negotiatorId = this.fixture.DataContext.Users.First().Id;

            var details = new CreateViewingCommand
            {
                ActivityId = activityId,
                AttendeesIds = attendeesIds,
                StartDate = this.date,
                EndDate = this.date.AddHours(1),
                InvitationText = StringExtension.GenerateMaxAlphanumericString(4000),
                NegotiatorId = negotiatorId,
                RequirementId = requirementId
            };

            this.CreateViewing(details);
        }

        [When(@"User creates viewing with mandatory fields using api")]
        public void CreateViewingWithMandatoryFieldsUsingApi()
        {
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            List<Guid> attendeesIds =
                this.scenarioContext.Get<Requirement>("Requirement").Contacts.Select(contact => contact.Id).ToList();
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;

            var details = new CreateViewingCommand
            {
                ActivityId = activityId,
                AttendeesIds = attendeesIds,
                StartDate = this.date,
                EndDate = this.date.AddHours(1),
                RequirementId = requirementId
            };

            this.CreateViewing(details);
        }

        [When(@"User creates viewing with invalid (.*) using api")]
        public void CreateViewingWithInvalidDataUsingApi(string data)
        {
            Guid activityId = data.Equals("activity") ? Guid.NewGuid() : this.scenarioContext.Get<Activity>("Activity").Id;
            List<Guid> attendeesIds = data.Equals("attendee")
                ? new List<Guid> { Guid.NewGuid() }
                : this.scenarioContext.Get<Requirement>("Requirement").Contacts.Select(contact => contact.Id).ToList();
            Guid requirementId = data.Equals("requirement")
                ? Guid.NewGuid()
                : this.scenarioContext.Get<Requirement>("Requirement").Id;
            Guid negotiatorId = this.fixture.DataContext.Users.First().Id;

            var details = new CreateViewingCommand
            {
                ActivityId = activityId,
                AttendeesIds = attendeesIds,
                StartDate = this.date,
                EndDate = this.date.AddHours(1),
                InvitationText = StringExtension.GenerateMaxAlphanumericString(4000),
                PostViewingComment = StringExtension.GenerateMaxAlphanumericString(4000),
                NegotiatorId = negotiatorId,
                RequirementId = requirementId
            };

            this.CreateViewing(details);
        }

        [When(@"User updates viewing")]
        public void UpdateViewing()
        {
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
                StartDate = this.date.AddHours(1),
                EndDate = this.date.AddHours(2)
            };

            this.UpdateViewing(details);
        }

        [When(@"User updates viewing with invalid (.*) data")]
        public void UpdateViewingWithInvalidData(string data)
        {
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
                StartDate = this.date.AddHours(1),
                EndDate = this.date.AddHours(2)
            };

            this.UpdateViewing(details);
        }

        [Then(@"Viewing details should be the same as already added")]
        public void CheckViewingDetails()
        {
            var viewing = JsonConvert.DeserializeObject<Viewing>(this.scenarioContext.GetResponseContent());
            Viewing expectedViewing = this.fixture.DataContext.Viewing.Single(v => v.Id.Equals(viewing.Id));

            expectedViewing.ShouldBeEquivalentTo(viewing, opt => opt
                .Excluding(v => v.Negotiator)
                .Excluding(v => v.Activity)
                .Excluding(v => v.Requirement));
        }

        [Then(@"Viewing details in requirement should be the same as added")]
        public void CompareRequirementViewings()
        {
            Viewing viewing = JsonConvert.DeserializeObject<Requirement>(this.scenarioContext.GetResponseContent()).Viewings.Single();
            Viewing expectedVieiwng = this.fixture.DataContext.Viewing.Single(v => v.Id.Equals(viewing.Id));

            viewing.ShouldBeEquivalentTo(expectedVieiwng, opt => opt
                .Excluding(v => v.Activity)
                .Excluding(v => v.Requirement)
                .Excluding(v => v.Negotiator));
        }

        private void CreateViewing(CreateViewingCommand command)
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, command);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        private void UpdateViewing(UpdateViewingCommand command)
        {
            string requestUrl = $"{ApiUrl}";
            HttpResponseMessage response = this.fixture.SendPutRequest(requestUrl, command);
            this.scenarioContext.SetHttpResponseMessage(response);
        }
    }
}
