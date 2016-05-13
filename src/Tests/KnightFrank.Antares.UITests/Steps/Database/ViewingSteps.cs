namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;

    using TechTalk.SpecFlow;

    [Binding]
    public class ViewingSteps
    {
        private readonly KnightFrankContext dataContext;
        private readonly ScenarioContext scenarioContext;

        public ViewingSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.dataContext = this.scenarioContext.Get<KnightFrankContext>("DataContext");
        }

        [Given(@"Viewing for requirement is defined")]
        public void CreateViewing()
        {
            var requirement = this.scenarioContext.Get<Requirement>("Requirement");
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;
            var viewing = new Viewing
            {
                ActivityId = activityId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1),
                InvitationText = "Invitation Text",
                NegotiatorId = this.dataContext.Users.First().Id,
                RequirementId = requirement.Id,
                Attendees = new List<Contact>
                {
                    requirement.Contacts[0],
                    requirement.Contacts[2]
                },
            };

            this.dataContext.Viewing.Add(viewing);
            this.dataContext.SaveChanges();
        }
    }
}
