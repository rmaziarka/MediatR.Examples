namespace KnightFrank.Antares.Domain.Offer.CommandHandlers
{
    using System;

    using Dal.Model.Offer;
    using Dal.Repository;
    using Common.BusinessValidators;
    using Common.Enums;
    using Commands;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Contacts;

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

            this.ValidateOfferEntities(offer, message);
            this.ValidateOfferEnums(message);
            this.ValidateOfferDates(message, offer);

            AutoMapper.Mapper.Map(message, offer);

            this.offerRepository.Save();

            return offer.Id;
        }

        private void ValidateOfferEntities(Offer offer, UpdateOfferCommand message)
        {
            this.entityValidator.EntityExists(offer, message.Id);

            this.entityValidator.EntityExists<Contact>(message.BrokerId);
            this.entityValidator.EntityExists<Company>(message.BrokerCompanyId);

            this.entityValidator.EntityExists<Contact>(message.LenderId);
            this.entityValidator.EntityExists<Company>(message.LenderCompanyId);

            this.entityValidator.EntityExists<Contact>(message.SurveyorId);
            this.entityValidator.EntityExists<Company>(message.SurveyorCompanyId);

            this.entityValidator.EntityExists<Contact>(message.AdditionalSurveyorId);
            this.entityValidator.EntityExists<Company>(message.AdditionalSurveyorCompanyId);
        }

        private void ValidateOfferEnums(UpdateOfferCommand message)
        {
            this.enumTypeItemValidator.ItemExists(EnumType.OfferStatus, message.StatusId);
            this.enumTypeItemValidator.ItemExists(EnumType.MortgageStatus, message.MortgageStatusId);
            this.enumTypeItemValidator.ItemExists(EnumType.MortgageSurveyStatus, message.MortgageSurveyStatusId);
            this.enumTypeItemValidator.ItemExists(EnumType.AdditionalSurveyStatus, message.AdditionalSurveyStatusId);
            this.enumTypeItemValidator.ItemExists(EnumType.SearchStatus, message.SearchStatusId);
            this.enumTypeItemValidator.ItemExists(EnumType.Enquiries, message.EnquiriesId);
        }

        private void ValidateOfferDates(UpdateOfferCommand message, Offer offer)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (message.OfferDate > offer.CreatedDate)
            {
                throw new BusinessValidationException(BusinessValidationMessage.OfferDateLessOrEqualToCreateDateMessage(offer.CreatedDate));
            }
            if (message.ExchangeDate < offer.CreatedDate)
            {
                throw new BusinessValidationException(BusinessValidationMessage.ExchangeDateGreaterOrEqualToCreateDateMessage(offer.CreatedDate));
            }
            if (message.CompletionDate < offer.CreatedDate)
            {
                throw new BusinessValidationException(BusinessValidationMessage.CompletionDateGreaterOrEqualToCreateDateMessage(offer.CreatedDate));
            }
            if (offer.MortgageSurveyDate != null && message.MortgageSurveyDate.HasValue)
            {
                if (message.MortgageSurveyDate.Value < message.OfferDate)
                {
                    throw new BusinessValidationException(BusinessValidationMessage.CompletionDateGreaterOrEqualToCreateDateMessage(offer.MortgageSurveyDate.Value));
                }
            }
            if (offer.AdditionalSurveyDate != null && message.AdditionalSurveyDate.HasValue)
            {
                if (message.AdditionalSurveyDate.Value < message.OfferDate)
                {
                    throw new BusinessValidationException(BusinessValidationMessage.CompletionDateGreaterOrEqualToCreateDateMessage(offer.AdditionalSurveyDate.Value));
                }
            }
        }
    }
}
