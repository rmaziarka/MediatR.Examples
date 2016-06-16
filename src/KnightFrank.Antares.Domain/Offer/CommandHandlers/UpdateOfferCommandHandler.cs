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
    using KnightFrank.Antares.Domain.Common.Enums;
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

            this.RemoveHoursFromDates(message);

            this.ValidateOfferDates(message, offer);

            List<EnumType> enumTypeItems = this.GetEnumTypeItems();
            if (this.offerProgressStatusHelper.IsOfferInAcceptedStatus(enumTypeItems, message.StatusId))
            {
                this.SetDefaultOfferProgressStatuses(message, enumTypeItems);
                this.ValidateOfferProgressEntities(message);
                this.ValidateOfferProgressEnums(message);
                this.ValidateOfferProgressDates(message);
            }
            else
            {
                this.offerProgressStatusHelper.KeepOfferProgressStatusesInMessage(offer, message);
                this.offerProgressStatusHelper.KeepOfferMortgageDetailsInMessage(offer, message);
                this.offerProgressStatusHelper.KeepOfferAdditionalSurveyInMessage(offer, message);
                this.offerProgressStatusHelper.KeepOfferOtherDetailsInMessage(offer, message);
            }

            AutoMapper.Mapper.Map(message, offer);

            this.offerRepository.Save();

            return offer.Id;
        }

        private void RemoveHoursFromDates(UpdateOfferCommand message)
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
            if (message.MortgageSurveyDate != null)
            {
                message.MortgageSurveyDate = message.MortgageSurveyDate.Value.Date;
            }
            if (message.AdditionalSurveyDate != null)
            {
                message.AdditionalSurveyDate = message.AdditionalSurveyDate.Value.Date;
            }
        }

        private UpdateOfferCommand SetDefaultOfferProgressStatuses(UpdateOfferCommand message, List<EnumType> enumStatusOfferTypes)
        {
            message.MortgageStatusId = message.MortgageStatusId ?? this.offerProgressStatusHelper.GetStatusId(DomainEnumType.MortgageStatus, MortgageStatus.Unknown.ToString(), enumStatusOfferTypes);
            message.MortgageSurveyStatusId = message.MortgageSurveyStatusId ?? this.offerProgressStatusHelper.GetStatusId(DomainEnumType.MortgageSurveyStatus, MortgageStatus.Unknown.ToString(), enumStatusOfferTypes);
            message.AdditionalSurveyStatusId = message.AdditionalSurveyStatusId ?? this.offerProgressStatusHelper.GetStatusId(DomainEnumType.AdditionalSurveyStatus, MortgageStatus.Unknown.ToString(), enumStatusOfferTypes);
            message.SearchStatusId = message.SearchStatusId ?? this.offerProgressStatusHelper.GetStatusId(DomainEnumType.SearchStatus, SearchStatus.NotStarted.ToString(), enumStatusOfferTypes);
            message.EnquiriesId = message.EnquiriesId ?? this.offerProgressStatusHelper.GetStatusId(DomainEnumType.Enquiries, Enquiries.NotStarted.ToString(), enumStatusOfferTypes);

            return message;
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

        private void ValidateOfferProgressEntities(UpdateOfferCommand message)
        {
            this.entityValidator.EntityExists<Contact>(message.BrokerId);
            this.entityValidator.EntityExists<Company>(message.BrokerCompanyId);

            this.entityValidator.EntityExists<Contact>(message.LenderId);
            this.entityValidator.EntityExists<Company>(message.LenderCompanyId);

            this.entityValidator.EntityExists<Contact>(message.SurveyorId);
            this.entityValidator.EntityExists<Company>(message.SurveyorCompanyId);

            this.entityValidator.EntityExists<Contact>(message.AdditionalSurveyorId);
            this.entityValidator.EntityExists<Company>(message.AdditionalSurveyorCompanyId);
        }

        private void ValidateOfferProgressEnums(UpdateOfferCommand message)
        {
            this.enumTypeItemValidator.ValidateMandatoryIfItemExists(DomainEnumType.MortgageStatus, message.MortgageStatusId);
            this.enumTypeItemValidator.ValidateMandatoryIfItemExists(DomainEnumType.MortgageSurveyStatus, message.MortgageSurveyStatusId);
            this.enumTypeItemValidator.ValidateMandatoryIfItemExists(DomainEnumType.AdditionalSurveyStatus, message.AdditionalSurveyStatusId);
            this.enumTypeItemValidator.ValidateMandatoryIfItemExists(DomainEnumType.SearchStatus, message.SearchStatusId);
            this.enumTypeItemValidator.ValidateMandatoryIfItemExists(DomainEnumType.Enquiries, message.EnquiriesId);
        }

        private void ValidateOfferProgressDates(UpdateOfferCommand message)
        {
            if (message.MortgageSurveyDate != null && (message.MortgageSurveyDate.Value.Date < message.OfferDate.Date))
            {
                throw new BusinessValidationException(BusinessValidationMessage.MortgageSurveyDateGreaterOrEqualToCreateDateMessage(message.MortgageSurveyDate.Value.Date));
            }
            if (message.AdditionalSurveyDate != null && (message.AdditionalSurveyDate.Value.Date < message.OfferDate.Date))
            {
                throw new BusinessValidationException(BusinessValidationMessage.AdditionalSurveyDateGreaterOrEqualToCreateDateMessage(message.AdditionalSurveyDate.Value.Date));
            }
        }
    }
}
