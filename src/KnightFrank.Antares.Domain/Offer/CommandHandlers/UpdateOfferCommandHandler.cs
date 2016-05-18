namespace KnightFrank.Antares.Domain.Offer.CommandHandlers
{
    using System;

    using Dal.Model.Offer;
    using Dal.Repository;
    using Common.BusinessValidators;
    using Common.Enums;
    using Commands;

    using MediatR;

    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand, Guid>
    {
        private readonly IGenericRepository<Offer> offerRepository;
        private readonly IEntityValidator entityValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;

        public UpdateOfferCommandHandler(
            IGenericRepository<Offer> offerRepository,
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator)
        {
            this.offerRepository = offerRepository;
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
        }

        public Guid Handle(UpdateOfferCommand message)
        {
            Offer offer = this.offerRepository.GetById(message.Id);

            this.entityValidator.EntityExists(offer, message.Id);
            this.enumTypeItemValidator.ItemExists(EnumType.OfferStatus, message.StatusId);

            AutoMapper.Mapper.Map(message, offer);

            this.offerRepository.Save();

            return offer.Id;
        }
    }
}
