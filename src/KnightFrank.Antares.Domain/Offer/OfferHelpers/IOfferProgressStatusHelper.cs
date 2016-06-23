namespace KnightFrank.Antares.Domain.Offer.OfferHelpers
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Offer;
    using KnightFrank.Antares.Domain.Offer.Commands;

    using EnumType = KnightFrank.Antares.Dal.Model.Enum.EnumType;
    using DomainEnumType = KnightFrank.Antares.Domain.Common.Enums.EnumType;

    public interface IOfferProgressStatusHelper
    {
        Offer SetOfferProgressStatuses(Offer offer, List<EnumType> enumOfferStatusTypes);

        Guid GetStatusId(DomainEnumType enumType, string enumTypeItemName, List<EnumType> enumOfferStatusTypes);

        bool IsOfferInAcceptedStatus(List<EnumType> enumOfferStatusTypes, Guid offerStatusId);

        void KeepOfferProgressStatusesInMessage(Offer offer, UpdateOfferCommand message);

        void KeepOfferMortgageDetailsInMessage(Offer offer, UpdateOfferCommand message);

        void KeepOfferAdditionalSurveyInMessage(Offer offer, UpdateOfferCommand message);

        void KeepOfferOtherDetailsInMessage(Offer offer, UpdateOfferCommand message);
    }
}
