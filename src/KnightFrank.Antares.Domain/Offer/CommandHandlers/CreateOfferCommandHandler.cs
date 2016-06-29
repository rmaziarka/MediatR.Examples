namespace KnightFrank.Antares.Domain.Offer.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Dal.Model.Property;
    using KnightFrank.Antares.Dal.Model.Property.Activities;
    using KnightFrank.Antares.Dal.Model.User;
    using KnightFrank.Antares.Dal.Repository;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Common;
    using KnightFrank.Antares.Domain.AttributeConfiguration.Enums;
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Offer.Commands;
    using KnightFrank.Antares.Domain.Offer.OfferHelpers;

    using MediatR;

    using EnumType = KnightFrank.Antares.Dal.Model.Enum.EnumType;
    using DomainEnumType = Common.Enums.EnumType;
    using EnumMapper = KnightFrank.Antares.Domain.Common.Enums.EnumMapper;
    using OfferType = KnightFrank.Antares.Dal.Model.Offer.OfferType;
    using RequirementType = KnightFrank.Antares.Domain.Common.Enums.RequirementType;

    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, Guid>
    {
        private readonly IGenericRepository<Offer> offerRepository;
        private readonly IReadGenericRepository<User> userRepository;
        private readonly IOfferProgressStatusHelper offerProgressStatusHelper;
        private readonly IEntityValidator entityValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IGenericRepository<EnumType> enumTypeRepository;
        private readonly IEntityMapper<Offer> offerEntityMapper;
        private readonly IGenericRepository<OfferType> offerTypeRepository;

        public CreateOfferCommandHandler(
            IGenericRepository<Offer> offerRepository,
            IReadGenericRepository<User> userRepository,
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IOfferProgressStatusHelper offerProgressStatusHelper, 
            IGenericRepository<EnumType> enumTypeRepository,
            IGenericRepository<OfferType> offerTypeRepository,
            IEntityMapper<Offer> offerEntityMapper)
        {
            this.offerRepository = offerRepository;
            this.userRepository = userRepository;
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.offerProgressStatusHelper = offerProgressStatusHelper;
            this.enumTypeRepository = enumTypeRepository;
            this.offerTypeRepository = offerTypeRepository;
            this.offerEntityMapper = offerEntityMapper;
        }

        public Guid Handle(CreateOfferCommand message)
        {
            this.entityValidator.EntityExists<Activity>(message.ActivityId);
            this.entityValidator.EntityExists<Requirement>(message.RequirementId);
            this.enumTypeItemValidator.ItemExists(DomainEnumType.OfferStatus, message.StatusId);
            this.RemoveHoursFromDates(message);

            Guid offerTypeId = this.GetOfferTypeByRequirementId(message.RequirementId).Id;

            var offer = new Offer
                            {
                                ActivityId = message.ActivityId,
                                RequirementId = message.RequirementId,
                                OfferTypeId = offerTypeId
                            };

            this.offerEntityMapper.MapAllowedValues(message, offer, PageType.Create);

            List<EnumType> enumTypeItems = this.GetEnumTypeItems();
            offer = this.offerProgressStatusHelper.SetOfferProgressStatuses(offer, enumTypeItems);

            Guid negotiatorId = this.userRepository.Get().First().Id;
            offer.NegotiatorId = negotiatorId;

            this.offerRepository.Add(offer);
            this.offerRepository.Save();

            return offer.Id;
        }

        private void RemoveHoursFromDates(CreateOfferCommand message)
        {
            message.OfferDate = message.OfferDate.Date;

            if (message.CompletionDate != null)
            {
                message.CompletionDate = message.CompletionDate.Value.Date;
            }
            if (message.ExchangeDate != null)
            {
                message.ExchangeDate = message.ExchangeDate.Value.Date;
            }
        }

        private List<EnumType> GetEnumTypeItems()
        {
            return this.enumTypeRepository
                .GetWithInclude(x => OfferProgressStatusHelper.OfferProgressStatusesEnumTypes.Contains(x.Code), x => x.EnumTypeItems)
                .ToList();
        }

        private OfferType GetOfferTypeByRequirementId(Guid requirementId)
        {
            //TODO change when we have requimentType (it should be define base on requirement)
            var requirementType = RequirementType.ResidentialLetting;

            Common.Enums.OfferType offerType = EnumMapper.GetOfferType(requirementType);
            return this.offerTypeRepository.FindBy(x => x.EnumCode == offerType.ToString()).Single();
        }
    }
}
