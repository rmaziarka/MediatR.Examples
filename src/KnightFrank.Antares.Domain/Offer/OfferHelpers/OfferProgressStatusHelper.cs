namespace KnightFrank.Antares.Domain.Offer.OfferHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Domain.Common.Enums;
    using KnightFrank.Antares.Domain.Offer.Commands;

    using EnumType = KnightFrank.Antares.Dal.Model.Enum.EnumType;
    using DomainEnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public class OfferProgressStatusHelper : IOfferProgressStatusHelper
    {
        public static List<string> OfferProgressStatusesEnumTypes => new List<string>
        {
            DomainEnumType.MortgageStatus.ToString(),
            DomainEnumType.MortgageSurveyStatus.ToString(),
            DomainEnumType.AdditionalSurveyStatus.ToString(),
            DomainEnumType.SearchStatus.ToString(),
            DomainEnumType.Enquiries.ToString(),
            DomainEnumType.OfferStatus.ToString()
        };

        public Offer SetOfferProgressStatuses(Offer offer, List<EnumType> enumOfferStatusTypes)
        {
            if (this.IsOfferInAcceptedStatus(enumOfferStatusTypes, offer.StatusId))
            {
                offer = this.SetDefaultOfferProgressStatuses(offer, enumOfferStatusTypes);
            }

            return offer;
        }

        private Offer SetDefaultOfferProgressStatuses(Offer offer, List<EnumType> enumStatusOfferTypes)
        {
            offer.MortgageStatusId = this.GetStatusId(DomainEnumType.MortgageStatus, MortgageStatus.Unknown.ToString(), enumStatusOfferTypes);
            offer.MortgageSurveyStatusId = this.GetStatusId(DomainEnumType.MortgageSurveyStatus, MortgageStatus.Unknown.ToString(), enumStatusOfferTypes);
            offer.AdditionalSurveyStatusId = this.GetStatusId(DomainEnumType.AdditionalSurveyStatus, MortgageStatus.Unknown.ToString(), enumStatusOfferTypes);
            offer.SearchStatusId = this.GetStatusId(DomainEnumType.SearchStatus, SearchStatus.NotStarted.ToString(), enumStatusOfferTypes);
            offer.EnquiriesId = this.GetStatusId(DomainEnumType.Enquiries, Enquiries.NotStarted.ToString(), enumStatusOfferTypes);

            return offer;
        }

        private Guid GetStatusId(DomainEnumType enumType, string enumTypeItemName, List<EnumType> enumOfferStatusTypes)
        {
            return enumOfferStatusTypes.Single(x => x.Code == enumType.ToString()).EnumTypeItems.Single(x => x.Code == enumTypeItemName).Id;
        }

        public bool IsOfferInAcceptedStatus(List<EnumType> enumOfferStatusTypes, Guid offerStatusId)
        {
            EnumType offerStatus = enumOfferStatusTypes.Single(x => x.Code == Common.Enums.EnumType.OfferStatus.ToString());
            EnumTypeItem acceptedStatus = offerStatus.EnumTypeItems.Single(x => x.Code == OfferStatus.Accepted.ToString());

            return offerStatusId == acceptedStatus.Id;
        }

        public void KeepOfferProgressStatusesInMessage(Offer offer, UpdateOfferCommand message)
        {
            message.MortgageStatusId = offer.MortgageStatusId;
            message.MortgageSurveyStatusId = offer.MortgageSurveyStatusId;
            message.AdditionalSurveyStatusId = offer.AdditionalSurveyStatusId;
            message.SearchStatusId = offer.SearchStatusId;
            message.EnquiriesId = offer.EnquiriesId;
            message.ContractApproved = offer.ContractApproved;
        }

        public void KeepOfferMortgageDetailsInMessage(Offer offer, UpdateOfferCommand message)
        {
            message.MortgageLoanToValue = offer.MortgageLoanToValue;

            message.BrokerId = offer.BrokerId;
            message.BrokerCompanyId = offer.BrokerCompanyId;

            message.LenderId = offer.LenderId;
            message.LenderCompanyId = offer.LenderCompanyId;

            message.MortgageSurveyDate = offer.MortgageSurveyDate;

            message.SurveyorId = offer.SurveyorId;
            message.SurveyorCompanyId = offer.SurveyorCompanyId;
        }

        public void KeepOfferAdditionalSurveyInMessage(Offer offer, UpdateOfferCommand message)
        {
            message.AdditionalSurveyDate = offer.AdditionalSurveyDate;

            message.AdditionalSurveyorId = offer.AdditionalSurveyorId;
            message.AdditionalSurveyorCompanyId = offer.AdditionalSurveyorCompanyId;
        }

        public void KeepOfferOtherDetailsInMessage(Offer offer, UpdateOfferCommand message)
        {
            message.ProgressComment = offer.ProgressComment;
        }
    }
}
