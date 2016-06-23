namespace KnightFrank.Antares.Api.IntegrationTests.Steps.Requierement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    using FluentAssertions;

    using KnightFrank.Antares.Api.IntegrationTests.Extensions;
    using KnightFrank.Antares.Api.IntegrationTests.Fixtures;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Domain.RequirementNote.Commands;

    using Newtonsoft.Json;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

    [Binding]
    public class RequirementNotesSteps
    {
        private const string ApiUrl = "/api/requirements/{0}/notes";
        private readonly BaseTestClassFixture fixture;

        private readonly ScenarioContext scenarioContext;

        public RequirementNotesSteps(BaseTestClassFixture fixture, ScenarioContext scenarioContext)
        {
            this.fixture = fixture;
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }
            this.scenarioContext = scenarioContext;
        }

        [Given(@"Requirement notes exists in database")]
        public void CretaeNoteInDatabase(Table notesTable)
        {
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            Requirement requirement = this.fixture.DataContext.Requirements.Single(req => req.Id.Equals(requirementId));

            List<RequirementNote> notes = notesTable.CreateSet<RequirementNote>().ToList();
            notes.ForEach(x => x.RequirementId = requirement.Id);

            requirement.RequirementNotes = notes;

            this.fixture.DataContext.SaveChanges();
            this.scenarioContext.Set(requirement, "Requirement");
        }

        [When(@"User creates note for requirement using api")]
        public void CreateNoteUsingApi()
        {
            Guid requirementId = this.scenarioContext.ContainsKey("Requirement")
                ? this.scenarioContext.Get<Requirement>("Requirement").Id
                : Guid.NewGuid();

            string requestUrl = string.Format($"{ApiUrl}", requirementId);

            var note = new CreateRequirementNoteCommand
            {
                Description = StringExtension.GenerateMaxAlphanumericString(4000),
                RequirementId = requirementId
            };

            HttpResponseMessage response = this.fixture.SendPostRequest(requestUrl, note);
            this.scenarioContext.SetHttpResponseMessage(response);
        }

        [Then(@"Note is saved in database")]
        public void CheckIfNoteSavedInDatabase()
        {
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            var expectedNote = JsonConvert.DeserializeObject<RequirementNote>(this.scenarioContext.GetResponseContent());

            RequirementNote note =
                this.fixture.DataContext.Requirements.Single(req => req.Id.Equals(requirementId)).RequirementNotes.Single();
            note.ShouldBeEquivalentTo(expectedNote, opt => opt.Excluding(n => n.User).Excluding(n => n.Requirement));
        }

        [Then(@"Notes should be the same as added")]
        public void CheckNote()
        {
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            IList<RequirementNote> resultNotes =
                JsonConvert.DeserializeObject<Requirement>(this.scenarioContext.GetResponseContent()).RequirementNotes.ToList();

            IList<RequirementNote> expectedNotes =
                this.fixture.DataContext.Requirements.Single(req => req.Id.Equals(requirementId)).RequirementNotes.ToList();

            expectedNotes.Should()
                         .Equal(resultNotes, (n1, n2) => n1.Description == n2.Description && n1.RequirementId == n2.RequirementId);
        }
    }
}
