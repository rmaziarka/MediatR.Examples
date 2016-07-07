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
	 	RequirementDocumentType,
	 	PropertyDocumentType,
	 	OfferStatus,
	 	UserType,
	 	ActivityDepartmentType,
	 	MortgageStatus,
	 	ClientCareStatus,
	 	MortgageSurveyStatus,
	 	AdditionalSurveyStatus,
	 	SearchStatus,
	 	Enquiries,
	 	SalutationFormat,
	 	MailingSalutation,
	 	EventSalutation,
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

	public enum RequirementDocumentType
	{
		TermsOfBusiness,
	}

	public enum PropertyDocumentType
	{
		Photograph,
		FloorPlan,
		Brochure,
		VideoTour,
	}

	public enum OfferStatus
	{
		New,
		Withdrawn,
		Rejected,
		Accepted,
	}

	public enum UserType
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

	public enum ClientCareStatus
	{
		MassiveActionClient,
		PrincipalClient,
		KeyClient,
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

	public enum SalutationFormat
	{
		MrJohnSmith,
		JohnSmithEsq,
	}

	public enum MailingSalutation
	{
		MailingFormal,
		MailingSemiformal,
		MailingInformal,
		MailingPersonal,
	}

	public enum EventSalutation
	{
		EventInvite,
		EventSemiformal,
		EventInformal,
		EventPersonal,
	}
}
