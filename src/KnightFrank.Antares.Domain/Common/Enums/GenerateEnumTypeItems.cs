// <copyright file="GenerateEnumTypeItems.tt">
// </copyright>
// ReSharper disable InconsistentNaming
namespace KnightFrank.Antares.Domain.Common.Enums
{
    public enum EnumType
    {
        EntityType,
        OwnershipType,
        ActivityStatus,
        Division,
        ActivityDocumentType,
        OfferStatus,
        ActivityUserType,
        ActivityDepartmentType,
    }


    public enum EntityType
    {
        Property,
        Requirement,
    }

    public enum OwnershipType
    {
        Freeholder,
        Leaseholder,
    }

    public enum ActivityStatus
    {
        PreAppraisal,
        MarketAppraisal,
        NotSelling,
    }

    public enum Division
    {
        Residential,
        Commercial,
    }

    public enum ActivityDocumentType
    {
        TermsOfBusiness,
        MarketingSignOff,
        CDDDocument,
        Photograph,
        FloorPlan,
        Brochure,
        VideoTour,
        EPC,
        GasCertificate,
    }

    public enum OfferStatus
    {
        New,
        Withdrawn,
        Rejected,
        Accepted,
    }

    public enum ActivityUserType
    {
        LeadNegotiator,
        SecondaryNegotiator,
    }

    public enum ActivityDepartmentType
    {
        Managing,
        Standard,
    }
}
