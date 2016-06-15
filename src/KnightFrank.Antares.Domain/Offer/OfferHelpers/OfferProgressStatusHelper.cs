namespace KnightFrank.Antares.Domain.Offer.OfferHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Domain.Common.Enums;

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
    }
}
