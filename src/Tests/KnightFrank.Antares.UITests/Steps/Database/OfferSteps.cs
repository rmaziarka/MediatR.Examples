namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Common.Enums;

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
        [When(@"Offer for requirement is defined")]
        public void CreateViewing()
        {
            Guid statusId =
                this.dataContext.EnumTypeItems.Single(
                    e => e.EnumType.Code.Equals(nameof(OfferStatus)) && e.Code.Equals(nameof(OfferStatus.New))).Id;
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;

            var offer = new Offer
            {
                //TODO improve selecting negotiator
                NegotiatorId = this.dataContext.Users.First().Id,
                ActivityId = activityId,
                Price = 1000,
                CompletionDate = DateTime.UtcNow,
                ExchangeDate = DateTime.UtcNow,
                OfferDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                RequirementId = requirementId,
                SpecialConditions = "Text",
                StatusId = statusId
            };

            this.dataContext.Offer.Add(offer);
            this.dataContext.SaveChanges();

            this.scenarioContext.Set(offer, "Offer");
        }
    }
}
