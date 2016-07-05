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

        #region Requirement ControlCode

        Requirement_RequirementType,
        Requirement_Description,
        Requirement_RentRange,
        Requirement_LocationRequirements,
        Requirement_Applicants,
        Requirement_Offers,
        Requirement_Viewings,
        Requirement_Attachments,
        Requirement_CreationDate,

        #endregion

        #region Offer ControlCode
        Offer_Status,
        Offer_MortgageStatus,
        Offer_MortgageSurveyStatus,
        Offer_SearchStatus,
        Offer_Enquiries,
        Offer_AdditionalSurveyStatus,
        Offer_Requirement,
        Offer_Activity,
        Offer_Negotiator,
        Offer_Broker,
        Offer_Lender,
        Offer_Surveyor,
        Offer_AdditionalSurveyor,
        Offer_ContractApproved,
        Offer_MortgageLoanToValue,
        Offer_Price,
        Offer_PricePerWeek,
        Offer_OfferDate,
        Offer_ExchangeDate,
        Offer_CompletionDate,
        Offer_MortgageSurveyDate,
        Offer_AdditionalSurveyDate,
        Offer_SpecialConditions,
        Offer_ProgressComment,
        Offer_CreatedDate,
        Offer_LastModifiedDate,
        Offer_Vendor_Solicitor,
        Offer_Applicant_Solicitor
        #endregion
    }
}