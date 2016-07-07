// <copyright file="enumTypeItems.tt">
// </copyright>
// ReSharper disable InconsistentNaming
module Antares.Common.Models.Enums
{
	export enum EnumType
	{
	 	EntityType,
	 	OwnershipType,
	 	ActivityStatus,
	 	Division,
	 	ActivityDocumentType,
	 	RequirementDocumentType,
	 	PropertyDocumentType,
	 	OfferStatus,
	 	ActivityUserType,
	 	ActivityDepartmentType,
	 	MortgageStatus,
	 	ClientCareStatus,
	 	MortgageSurveyStatus,
	 	AdditionalSurveyStatus,
	 	SearchStatus,
	 	Enquiries,
	 	SalutationFormat,
	 	ActivitySource,
	 	ActivitySellingReason,
	 	DisposalType,
	 	Decoration,
	 }

	
	export enum EntityType
	{
		Property,
		Requirement,
	}

	export enum OwnershipType
	{
		Freeholder,
		Leaseholder,
	}

	export enum ActivityStatus
	{
		PreAppraisal,
		MarketAppraisal,
		NotSelling,
	}

	export enum Division
	{
		Residential,
		Commercial,
	}

	export enum ActivityDocumentType
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

	export enum RequirementDocumentType
	{
		TermsOfBusiness,
	}

	export enum PropertyDocumentType
	{
		Photograph,
		FloorPlan,
		Brochure,
		VideoTour,
	}

	export enum OfferStatus
	{
		New,
		Withdrawn,
		Rejected,
		Accepted,
	}

	export enum ActivityUserType
	{
		LeadNegotiator,
		SecondaryNegotiator,
	}

	export enum ActivityDepartmentType
	{
		Managing,
		Standard,
	}

	export enum MortgageStatus
	{
		Unknown,
		NotRequiredCashFromSale,
		NotRequiredCashInBank,
		InProgress,
		Agreed,
	}

	export enum ClientCareStatus
	{
		MassiveActionClient,
		PrincipalClient,
		KeyClient,
	}

	export enum MortgageSurveyStatus
	{
		Unknown,
		NotRequired,
		Complete,
		Outstanding,
	}

	export enum AdditionalSurveyStatus
	{
		Unknown,
		NotRequired,
		InProgress,
		Complete,
	}

	export enum SearchStatus
	{
		NotStarted,
		AppliedFor,
		Complete,
	}

	export enum Enquiries
	{
		NotStarted,
		Sent,
		Complete,
	}

	export enum SalutationFormat
	{
		MrJohnSmith,
		JohnSmithEsq,
	}

	export enum ActivitySource
	{
		KFContactsRegister,
		KFPR,
		Knightfrank,
		KnightfrankGlobalSearch,
		DirectEmail,
		DirectPhoneCall,
		DeveloperWebsite,
		LeadGenerationCanvassingActivities,
		OnTheMarket,
		OtherInternetPortal,
		OtherPortalGlobrix,
		OtherPortalPrimelocation,
		POD,
		TBSReferralAdvert,
		TBSReferralBank,
		TBSReferralPastClient,
		TBSReferralKF,
		TBSReferralProfessionalContact,
		TBSReferralWebsite,
		MagazineA,
		MagazineB,
		MagazineC,
		NewspaperA,
		NewspaperB,
		NewspaperC,
	}

	export enum ActivitySellingReason
	{
		Upsizing,
		DebtFinancialDifficulty,
		Divorce,
		Downsizing,
		FamilySize,
		PrivateDeveloper,
		Probate,
		ReceiverDisposal,
		RegularPropertyValueAppraisal,
		Relocation,
		Retiring,
		SurpliceToRequirement,
	}

	export enum DisposalType
	{
		PrivateTreaty,
		FormalTender,
		Auction,
	}

	export enum Decoration
	{
		Unmodernised,
		Fair,
		Good,
		VeryGood,
	}
}
