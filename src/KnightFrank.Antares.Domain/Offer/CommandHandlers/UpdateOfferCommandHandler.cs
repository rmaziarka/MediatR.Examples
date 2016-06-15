namespace KnightFrank.Antares.Domain.Offer.CommandHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Dal.Model.Offer;
    using Dal.Repository;
    using Common.BusinessValidators;
    using Commands;

    using KnightFrank.Antares.Dal.Model.Company;
    using KnightFrank.Antares.Dal.Model.Contacts;
    using KnightFrank.Antares.Domain.Offer.OfferHelpers;

    using MediatR;

    using EnumType = KnightFrank.Antares.Dal.Model.Enum.EnumType;
    using DomainEnumType = Common.Enums.EnumType;

    public class UpdateOfferCommandHandler : IRequestHandler<UpdateOfferCommand, Guid>
    {
        private readonly IGenericRepository<Offer> offerRepository;
        private readonly IEntityValidator entityValidator;
        private readonly IEnumTypeItemValidator enumTypeItemValidator;
        private readonly IOfferProgressStatusHelper offerProgressStatusHelper;
        private readonly IGenericRepository<EnumType> enumTypeRepository;

        public UpdateOfferCommandHandler(
            IGenericRepository<Offer> offerRepository,
            IEntityValidator entityValidator,
            IEnumTypeItemValidator enumTypeItemValidator,
            IGenericRepository<EnumType> enumTypeRepository,
            IOfferProgressStatusHelper offerProgressStatusHelper)
        {
            this.offerRepository = offerRepository;
            this.entityValidator = entityValidator;
            this.enumTypeItemValidator = enumTypeItemValidator;
            this.enumTypeRepository = enumTypeRepository;
            this.offerProgressStatusHelper = offerProgressStatusHelper;
        }

        public Guid Handle(UpdateOfferCommand message)
        {
            Offer offer = this.offerRepository.GetById(message.Id);

            this.entityValidator.EntityExists(offer, message.Id);
            this.enumTypeItemValidator.ItemExists(DomainEnumType.OfferStatus, message.StatusId);

            this.ValidateOfferDates(message, offer);

            AutoMapper.Mapper.Map(message, offer);

            List<EnumType> enumTypeItems = this.GetEnumTypeItems();
            if (this.offerProgressStatusHelper.IsOfferInAcceptedStatus(enumTypeItems, offer.StatusId))
            {
                this.ValidateOfferProgressEntities(offer);
                this.ValidateOfferProgressEnums(offer);
                this.ValidateOfferProgressDates(offer);
            }

            this.offerRepository.Save();

            return offer.Id;
        }

        private List<EnumType> GetEnumTypeItems()
        {
            return this.enumTypeRepository
                .GetWithInclude(x => OfferProgressStatusHelper.OfferProgressStatusesEnumTypes.Contains(x.Code), x => x.EnumTypeItems)
                .ToList();
        }

        private void ValidateOfferDates(UpdateOfferCommand message, Offer offer)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (message.OfferDate.Date > offer.CreatedDate.Date)
            {
                throw new BusinessValidationException(BusinessValidationMessage.OfferDateLessOrEqualToCreateDateMessage(offer.CreatedDate.Date));
            }
            if (message.ExchangeDate.HasValue && (message.ExchangeDate.Value.Date < offer.CreatedDate.Date))
            {
                throw new BusinessValidationException(BusinessValidationMessage.ExchangeDateGreaterOrEqualToCreateDateMessage(offer.CreatedDate.Date));
            }
            if (message.CompletionDate.HasValue && (message.CompletionDate.Value.Date < offer.CreatedDate.Date))
            {
                throw new BusinessValidationException(BusinessValidationMessage.CompletionDateGreaterOrEqualToCreateDateMessage(offer.CreatedDate.Date));
            }
        }

        private void ValidateOfferProgressEntities(Offer offer)
        {
            this.entityValidator.EntityExists<Contact>(offer.BrokerId);
            this.entityValidator.EntityExists<Company>(offer.BrokerCompanyId);

            this.entityValidator.EntityExists<Contact>(offer.LenderId);
            this.entityValidator.EntityExists<Company>(offer.LenderCompanyId);

            this.entityValidator.EntityExists<Contact>(offer.SurveyorId);
            this.entityValidator.EntityExists<Company>(offer.SurveyorCompanyId);

            this.entityValidator.EntityExists<Contact>(offer.AdditionalSurveyorId);
            this.entityValidator.EntityExists<Company>(offer.AdditionalSurveyorCompanyId);
        }

        private void ValidateOfferProgressEnums(Offer offer)
        {
            this.enumTypeItemValidator.ItemExists(DomainEnumType.MortgageStatus, offer.MortgageStatusId);
            this.enumTypeItemValidator.ItemExists(DomainEnumType.MortgageSurveyStatus, offer.MortgageSurveyStatusId);
            this.enumTypeItemValidator.ItemExists(DomainEnumType.AdditionalSurveyStatus, offer.AdditionalSurveyStatusId);
            this.enumTypeItemValidator.ItemExists(DomainEnumType.SearchStatus, offer.SearchStatusId);
            this.enumTypeItemValidator.ItemExists(DomainEnumType.Enquiries, offer.EnquiriesId);
        }

        private void ValidateOfferProgressDates(Offer offer)
        {
            if (offer.MortgageSurveyDate != null && (offer.MortgageSurveyDate.Value.Date < offer.OfferDate.Date))
            {
                throw new BusinessValidationException(BusinessValidationMessage.MortgageSurveyDateGreaterOrEqualToCreateDateMessage(offer.MortgageSurveyDate.Value.Date));
            }
            if (offer.AdditionalSurveyDate != null && (offer.AdditionalSurveyDate.Value.Date < offer.OfferDate.Date))
            {
                throw new BusinessValidationException(BusinessValidationMessage.AdditionalSurveyDateGreaterOrEqualToCreateDateMessage(offer.AdditionalSurveyDate.Value.Date));
            }
        }
    }
}
