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
	 	UserType,
	 	ActivityDepartmentType,
	 	MortgageStatus,
	 	ChainMortgageStatus,
	 	ClientCareStatus,
	 	CompanyType,
	 	CompanyCategory,
	 	MortgageSurveyStatus,
	 	ChainMortgageSurveyStatus,
	 	AdditionalSurveyStatus,
	 	SearchStatus,
	 	ChainSearchStatus,
	 	Enquiries,
	 	ChainEnquiries,
	 	ChainContractAgreedStatus,
	 	SalutationFormat,
	 	ActivitySource,
	 	ActivitySellingReason,
	 	DisposalType,
	 	Decoration,
	 	MailingSalutation,
	 	EventSalutation,
	 	ActivityPriceType,
	 	ActivityMatchFlexPrice,
	 	ActivityMatchFlexRent,
	 	RentPaymentPeriod,
	 	SalesBoardType,
	 	SalesBoardStatus,
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
		ForSaleUnavailable,
		ToLetUnavailable,
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

	export enum UserType
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
		Agreed,
		Unknown,
		NotRequiredCashFromSale,
		NotRequiredCashInBank,
		InProgress,
	}

	export enum ChainMortgageStatus
	{
		Unknown,
		Complete,
		Outstanding,
		NotRequired,
	}

	export enum ClientCareStatus
	{
		MassiveActionClient,
		PrincipalClient,
		KeyClient,
	}

	export enum CompanyType
	{
		KnightFrankGroup,
		KnightFrankAffiliates,
	}

	export enum CompanyCategory
	{
		Accountants,
		AgriculturalAndFarming,
		Apparel,
		ArchitectsDesign,
		AssetManagement,
		Banks,
		BooksStationery,
		BuildingAndConstruction,
		BusinessServices,
		BuyingAgent,
		CareHome,
		CharitiesAndChurches,
		Chemist,
		DeptVariety,
		EateriesBars,
		Education,
		ElectricalTelephones,
		EmploymentAgency,
		EquestrianStud,
		FamilyOffice,
		Finance,
		FoodDrinkRetailers,
		GeneralServices,
		Hotel,
		Industrial,
		Insurance,
		InvestorsFundManagers,
		Legal,
		LocalAuthorityGovernment,
		MediaAndPublishing,
		MedicalHealthcare,
		MultiFamilyOffice,
		OilAndGas,
		Other,
		PropertyCompany,
		PropertyConsultantsSurveyors,
		RenewablesSustainability,
		Retailers,
		Supermarkets,
		TelecommunicationsTechnology,
		TourismLeisure,
		Transport,
		TravelAgents,
		Utilities,
		Wholesale,
	}

	export enum MortgageSurveyStatus
	{
		Unknown,
		NotRequired,
		Complete,
		Outstanding,
	}

	export enum ChainMortgageSurveyStatus
	{
		Unknown,
		Complete,
		Outstanding,
		NotRequired,
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

	export enum ChainSearchStatus
	{
		Complete,
		Outstanding,
	}

	export enum Enquiries
	{
		NotStarted,
		Sent,
		Complete,
	}

	export enum ChainEnquiries
	{
		Complete,
		Outstanding,
	}

	export enum ChainContractAgreedStatus
	{
		Complete,
		Outstanding,
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

	export enum MailingSalutation
	{
		MailingFormal,
		MailingSemiformal,
		MailingInformal,
		MailingPersonal,
	}

	export enum EventSalutation
	{
		EventInvite,
		EventSemiformal,
		EventInformal,
		EventPersonal,
	}

	export enum ActivityPriceType
	{
		AskingPrice,
		PriceOnApplication,
		GuidePrice,
		OffersInRegionOf,
		OffersInExcessOf,
		FixedPrice,
		PriceReducedTo,
	}

	export enum ActivityMatchFlexPrice
	{
		MinimumPrice,
		Percentage,
	}

	export enum ActivityMatchFlexRent
	{
		MinimumRent,
		Percentage,
	}

	export enum RentPaymentPeriod
	{
		Weekly,
		Monthly,
	}

	export enum SalesBoardType
	{
		None,
		Flag,
		VBoard,
	}

	export enum SalesBoardStatus
	{
		ForSale,
		Sold,
		SoldSTC,
		UnderOffer,
		ToLet,
		Let,
	}
}
