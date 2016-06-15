namespace KnightFrank.Antares.Domain.Offer.OfferHelpers
{
    using System;
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Offer;

    using EnumType = KnightFrank.Antares.Dal.Model.Enum.EnumType;
    using DomainEnumType = Common.Enums.EnumType;

    public interface IOfferProgressStatusHelper
    {
        Offer SetOfferProgressStatuses(Offer offer, List<EnumType> enumOfferStatusTypes);

        bool IsOfferInAcceptedStatus(List<EnumType> enumOfferStatusTypes, Guid offerStatusId);

        Offer SetEmptyOfferProgressStatuses(Offer offer);

        Offer SetEmptyOfferMortgageDetails(Offer offer);

        Offer SetEmptyOfferAdditionalSurvey(Offer offer);

        Offer SetEmptyOfferOtherDetails(Offer offer);
    }
}
