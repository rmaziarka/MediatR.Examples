// ReSharper disable InconsistentNaming
namespace KnightFrank.Antares.Domain.AttributeConfiguration.Enums
{
    public enum ControlCode
    {
        #region Activity ControlCode
        ActivityType,
        ActivityStatus,
        Vendors,
        Property,
        Negotiators,
        Landlords,
        Departments,
        AskingPrice,
        ShortLetPricePerWeek,
        CreationDate,
        MarketAppraisalPrice,
        RecommendedPrice,
        VendorEstimatedPrice,
        Offers,
        Viewings,
        Attachments,
        #endregion

        #region Offer ControlCode
        Offer_Status,
        Offer_OfferType,
        Offer_Requirement,
        Offer_Activity,
        Offer_Negotiator,
        Offer_Price,
        Offer_PricePerWeek,
        Offer_OfferDate,
        Offer_ExchangeDate,
        Offer_CompletionDate,
        Offer_SpecialConditions,
        Offer_CreatedDate,
        Offer_LastModifiedDate
        #endregion
    }
}