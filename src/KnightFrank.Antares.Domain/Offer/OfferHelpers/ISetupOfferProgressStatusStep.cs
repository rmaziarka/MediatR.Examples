namespace KnightFrank.Antares.Domain.Offer.OfferHelpers
{
    using System.Collections.Generic;

    using KnightFrank.Antares.Dal.Model.Enum;
    using KnightFrank.Antares.Dal.Model.Offer;

    public interface ISetupOfferProgressStatusStep
    {
        Offer SetOfferProgressStatuses(Offer offer, List<EnumType> enumOfferStatusTypes);
        List<string> OfferProgressStatusesEnumTypes { get; }
    }
}