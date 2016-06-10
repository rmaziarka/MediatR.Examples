namespace KnightFrank.Antares.Domain.Offer.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Offer.Commands;

    using MediatR;

    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, Guid>
    {
        private readonly IGenericRepository<Offer> offerRepository;
        private readonly IReadGenericRepository<User> userRepository;
        private readonly IGenericRepository<EnumType> enumTypeRepository;
        private readonly IEntityValidator entityValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;

        public CreateOfferCommandHandler(
            IGenericRepository<Offer> offerRepository,
            IReadGenericRepository<User> userRepository,
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IGenericRepository<EnumType> enumTypeRepository)
        {
            this.offerRepository = offerRepository;
            this.userRepository = userRepository;
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.enumTypeRepository = enumTypeRepository;
        }

        public Guid Handle(CreateOfferCommand message)
        {
            this.entityValidator.EntityExists<Activity>(message.ActivityId);
            this.entityValidator.EntityExists<Requirement>(message.RequirementId);
            this.enumTypeItemValidator.ItemExists(Common.Enums.EnumType.OfferStatus, message.StatusId);

            var offer = AutoMapper.Mapper.Map<Offer>(message);

            offer = this.SetOfferProgressStatuses(message.StatusId, offer);

            Guid negotiatorId = this.userRepository.Get().First().Id;
            offer.NegotiatorId = negotiatorId;

            this.offerRepository.Add(offer);
            this.offerRepository.Save();

            return offer.Id;
        }

        private Offer SetOfferProgressStatuses(Guid offerStatusId, Offer offer)
        {
            EnumType offerStatus = this.enumTypeRepository.GetWithInclude(x => x.Code == "OfferStatus", x => x.EnumTypeItems).Single();
            EnumTypeItem acceptedStatus = offerStatus.EnumTypeItems.Single(x => x.Code == "Accepted");

            if (offerStatusId == acceptedStatus.Id)
            {
                var progressStatusesEnumTypes = new List<string>
                {
                    "MortgageStatus",
                    "MortgageSurveyStatus",
                    "AdditionalSurveyStatus",
                    "SearchStatus",
                    "Enquiries"
                };

                List<EnumType> offerProgressStatusesItems =
                    this.enumTypeRepository
                    .GetWithInclude(x => progressStatusesEnumTypes.Contains(x.Code), x => x.EnumTypeItems)
                    .ToList();

                offer.MortgageStatusId = this.GetStatusId("MortgageStatus", "Unknown", offerProgressStatusesItems);
                offer.MortgageSurveyStatusId = this.GetStatusId("MortgageSurveyStatus", "Unknown", offerProgressStatusesItems);
                offer.AdditionalSurveyStatusId = this.GetStatusId("AdditionalSurveyStatus", "Unknown", offerProgressStatusesItems);
                offer.SearchStatusId = this.GetStatusId("SearchStatus", "NotStarted", offerProgressStatusesItems);
                offer.EnquiriesId = this.GetStatusId("Enquiries", "NotStarted", offerProgressStatusesItems);
            }
            else
            {
                offer.MortgageStatusId = null;
                offer.MortgageSurveyStatusId = null;
                offer.AdditionalSurveyStatusId = null;
                offer.SearchStatusId = null;
                offer.EnquiriesId = null;
            }

            return offer;
        }

        private Guid GetStatusId(string enumTypeName, string enumTypeItemName, List<EnumType> offerProgressStatusesItems)
        {
            return offerProgressStatusesItems.Single(x => x.Code == enumTypeName).EnumTypeItems.Single(x => x.Code == enumTypeItemName).Id;
        }
    }
}
