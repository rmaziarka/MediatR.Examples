namespace KnightFrank.Antares.UITests.Steps.Database
{
    using System;
    using System.Linq;

    using KnightFrank.Antares.Dal;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.UITests.Pages.Panels;

    using TechTalk.SpecFlow;
    using TechTalk.SpecFlow.Assist;

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
        public void CreateViewing(Table table)
        {
            var details = table.CreateInstance<OfferData>();
            Guid statusId =
                this.dataContext.EnumTypeItems.Single(
                    i => i.EnumType.Code.Equals(nameof(OfferStatus)) && i.Code.Equals(details.Status)).Id;
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
