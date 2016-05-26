namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;

    using TechTalk.SpecFlow;

    [Binding]
    public class OfferSteps
    {
        private readonly KnightFrankContext dataContext;
        private readonly ScenarioContext scenarioContext;

        public OfferSteps(ScenarioContext scenarioContext)
        {
            if (scenarioContext == null)
            {
                throw new ArgumentNullException(nameof(scenarioContext));
            }

            this.scenarioContext = scenarioContext;
            this.dataContext = this.scenarioContext.Get<KnightFrankContext>("DataContext");
        }

        [Given(@"Offer for requirement is defined")]
        public void CreateViewing()
        {
            Guid statusId = this.dataContext.EnumTypeItems.Single(i => i.EnumType.Code.Equals("OfferStatus") && i.Code.Equals("New")).Id;
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;

            var offer = new Offer
            {
                ActivityId = activityId,
                //TODO improve selecting negotiator
                NegotiatorId = this.dataContext.Users.First().Id,
                Price = 1000,
                CompletionDate = DateTime.UtcNow,
                ExchangeDate = DateTime.UtcNow,
                OfferDate = DateTime.UtcNow,
                RequirementId = requirementId,
                SpecialConditions = "Text",
                StatusId = statusId
            };

            this.dataContext.Offer.Add(offer);
            this.dataContext.SaveChanges();
        }
    }
}
