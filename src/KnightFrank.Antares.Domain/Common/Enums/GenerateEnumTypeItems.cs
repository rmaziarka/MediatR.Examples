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
		ForSaleUnavailable,
		ToLetUnavailable,
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
		Agreed,
		Unknown,
		NotRequiredCashFromSale,
		NotRequiredCashInBank,
		InProgress,
	}

	public enum ChainMortgageStatus
	{
		Unknown,
		Complete,
		Outstanding,
		NotRequired,
	}

	public enum ClientCareStatus
	{
		MassiveActionClient,
		PrincipalClient,
		KeyClient,
	}

	public enum CompanyType
	{
		KnightFrankGroup,
		KnightFrankAffiliates,
	}

	public enum CompanyCategory
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

	public enum MortgageSurveyStatus
	{
		Unknown,
		NotRequired,
		Complete,
		Outstanding,
	}

	public enum ChainMortgageSurveyStatus
	{
		Unknown,
		Complete,
		Outstanding,
		NotRequired,
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

	public enum ChainSearchStatus
	{
		Complete,
		Outstanding,
	}

	public enum Enquiries
	{
		NotStarted,
		Sent,
		Complete,
	}

	public enum ChainEnquiries
	{
		Complete,
		Outstanding,
	}

	public enum ChainContractAgreedStatus
	{
		Complete,
		Outstanding,
	}

	public enum SalutationFormat
	{
		MrJohnSmith,
		JohnSmithEsq,
	}

	public enum ActivitySource
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

	public enum ActivitySellingReason
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

	public enum DisposalType
	{
		PrivateTreaty,
		FormalTender,
		Auction,
	}

	public enum Decoration
	{
		Unmodernised,
		Fair,
		Good,
		VeryGood,
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

	public enum ActivityPriceType
	{
		AskingPrice,
		PriceOnApplication,
		GuidePrice,
		OffersInRegionOf,
		OffersInExcessOf,
		FixedPrice,
		PriceReducedTo,
	}

	public enum ActivityMatchFlexPrice
	{
		MinimumPrice,
		Percentage,
	}

	public enum ActivityMatchFlexRent
	{
		MinimumRent,
		Percentage,
	}

	public enum RentPaymentPeriod
	{
		Weekly,
		Monthly,
	}

	public enum SalesBoardType
	{
		None,
		Flag,
		VBoard,
	}

	public enum SalesBoardStatus
	{
		ForSale,
		Sold,
		SoldSTC,
		UnderOffer,
		ToLet,
		Let,
	}
}
