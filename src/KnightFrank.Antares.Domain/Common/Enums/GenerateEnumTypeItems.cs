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
	 	MortgageStatus,
	 	MortgageSurveyStatus,
	 	AdditionalSurveyStatus,
	 	SearchStatus,
	 	Enquiries,
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

	public enum MortgageStatus
	{
		Unknown,
		NotRequiredCashFromSale,
		NotRequiredCashInBank,
		InProgress,
		Agreed,
	}

	public enum MortgageSurveyStatus
	{
		Unknown,
		NotRequired,
		Complete,
		Outstanding,
	}

	public enum AdditionalSurveyStatus
	{
		Unknown,
		NotRequired,
		InProgress,
		Complete,
	}

	public enum SearchStatus
	{
		NotStarted,
		AppliedFor,
		Complete,
	}

	public enum Enquiries
	{
		NotStarted,
		Sent,
		Complete,
	}
}
