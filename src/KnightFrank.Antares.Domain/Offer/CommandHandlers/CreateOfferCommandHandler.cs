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
    using KnightFrank.Antares.Domain.Common.BusinessValidators;
    using KnightFrank.Antares.Domain.Offer.Commands;
    using KnightFrank.Antares.Domain.Offer.OfferHelpers;

    using MediatR;

    using EnumType = KnightFrank.Antares.Dal.Model.Enum.EnumType;
    using DomainEnumType = Common.Enums.EnumType;

    public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, Guid>
    {
        private readonly IGenericRepository<Offer> offerRepository;
        private readonly IReadGenericRepository<User> userRepository;
        private readonly IOfferProgressStatusHelper offerProgressStatusHelper;
        private readonly IEntityValidator entityValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IGenericRepository<EnumType> enumTypeRepository;
        

        public CreateOfferCommandHandler(
            IGenericRepository<Offer> offerRepository,
            IReadGenericRepository<User> userRepository,
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IOfferProgressStatusHelper offerProgressStatusHelper, 
            IGenericRepository<EnumType> enumTypeRepository)
        {
            this.offerRepository = offerRepository;
            this.userRepository = userRepository;
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.offerProgressStatusHelper = offerProgressStatusHelper;
            this.enumTypeRepository = enumTypeRepository;
        }

        public Guid Handle(CreateOfferCommand message)
        {
            this.entityValidator.EntityExists<Activity>(message.ActivityId);
            this.entityValidator.EntityExists<Requirement>(message.RequirementId);
            this.enumTypeItemValidator.ItemExists(DomainEnumType.OfferStatus, message.StatusId);
            
            var offer = AutoMapper.Mapper.Map<Offer>(message);

            List<EnumType> enumTypeItems = this.GetEnumTypeItems();
            offer = this.offerProgressStatusHelper.SetOfferProgressStatuses(offer, enumTypeItems);

            Guid negotiatorId = this.userRepository.Get().First().Id;
            offer.NegotiatorId = negotiatorId;

            this.offerRepository.Add(offer);
            this.offerRepository.Save();

            return offer.Id;
        }

        private List<EnumType> GetEnumTypeItems()
        {
            return this.enumTypeRepository
                .GetWithInclude(x => OfferProgressStatusHelper.OfferProgressStatusesEnumTypes.Contains(x.Code), x => x.EnumTypeItems)
                .ToList();
        }
    }
}
