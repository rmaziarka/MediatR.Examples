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

    using OfferType = KnightFrank.Antares.Domain.Common.Enums.OfferType;

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
        public void CreateOffer(Table table)
        {
            var details = table.CreateInstance<OfferData>();
            OfferType offerType = details.Type.Equals("Residential Sale") ? OfferType.ResidentialSale : OfferType.ResidentialLetting;

            Guid offerTypeId = this.dataContext.OfferTypes.First(ot => ot.EnumCode.Equals(offerType.ToString())).Id;
            Guid statusId =
                this.dataContext.EnumTypeItems.Single(
                    i => i.EnumType.Code.Equals(nameof(OfferStatus)) && i.Code.Equals(details.Status)).Id;
            Guid requirementId = this.scenarioContext.Get<Requirement>("Requirement").Id;
            Guid activityId = this.scenarioContext.Get<Activity>("Activity").Id;

            var offer = new Offer
            {
                //TODO improve selecting negotiator
                NegotiatorId = this.dataContext.Users.First().Id,
                ActivityId = activityId,
                CompletionDate = DateTime.UtcNow,
                ExchangeDate = DateTime.UtcNow,
                OfferDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                LastModifiedDate = DateTime.UtcNow,
                RequirementId = requirementId,
                SpecialConditions = "Text",
                StatusId = statusId,
                OfferTypeId = offerTypeId
            };

            if (offerType.Equals(OfferType.ResidentialSale))
            {
                offer.Price = 1000;
            }
            else
            {
                offer.PricePerWeek = 1000;
            }

            this.dataContext.Offer.Add(offer);
            this.dataContext.SaveChanges();

            this.scenarioContext.Set(offer, "Offer");
        }
    }
}
