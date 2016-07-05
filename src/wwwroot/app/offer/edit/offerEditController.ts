/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;
    import PubSub = Core.PubSub;
    import CloseSidePanelEvent = Common.Component.CloseSidePanelEvent;
    import Enums = Common.Models.Enums;
    import OpenCompanyContactEditPanelEvent = Antares.Attributes.OpenCompanyContactEditPanelEvent;
    import CompanyContactType = Antares.Common.Models.Enums.CompanyContactType;
    import ICompanyContactEditControlSchema = Antares.Attributes.ICompanyContactEditControlSchema;
    import CompanyContactConnection = Antares.Common.Models.Business.CompanyContactRelation;

    export class OfferEditController extends Core.WithPanelsBaseController {
        // bindings
        offer: Business.Offer;
        config: IOfferEditConfig;

        public offerOriginal: Business.Offer;

        public enumTypeMortgageStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.MortgageStatus;
        public enumTypeMortgageSurveyStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.MortgageSurveyStatus;
        public enumTypeAdditionalSurveyStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.AdditionalSurveyStatus;
        public enumTypeSearchStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.SearchStatus;
        public enumTypeEnquiriesStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.Enquiries;
        public enumTypeOfferStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.OfferStatus;
        public companyContactType = Antares.Common.Models.Enums.CompanyContactType;

        sidePanelSelectedCompanyContacts: Business.CompanyContact[];

        offerStatuses: any;
        mortgageStatuses: any;
        mortgageSurveyStatuses: any;
        additionalSurveyStatuses: any;
        searchStatuses: any;
        enquiriesStatuses: any;

        editOfferForm: any;

        mortgageSurveyDateOpen: boolean = false;
        additionalSurveyDateOpen: boolean = false;
        exchangeDateOpen: boolean = false;
        completionDateOpen: boolean = false;
        
        isBrokerEditPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isLenderEditPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isSurveyorEditPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isAdditionalSurveyorEditPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;

        contactToSelect: string = '';

        brokerCompanyContact: CompanyContactConnection = null;
        lenderCompanyContact: CompanyContactConnection = null;
        surveyorCompanyContact: CompanyContactConnection = null;
        additionalSurveyorCompanyContact: CompanyContactConnection = null;

        // controls
        controlSchemas: any = {
            broker: <ICompanyContactEditControlSchema>{
                formName: 'brokerForm',
                controlId: 'broker',
                translationKey: 'OFFER.EDIT.BROKER',
                emptyTranslationKey: 'OFFER.EDIT.NO_BROKER'
            },
            lender: <ICompanyContactEditControlSchema>{
                formName: 'lenderForm',
                controlId: 'lender',
                translationKey: 'OFFER.EDIT.LENDER',
                emptyTranslationKey: 'OFFER.EDIT.NO_LENDER'
            },
            surveyor: <ICompanyContactEditControlSchema>{
                formName: 'surveyorForm',
                controlId: 'surveyor',
                translationKey: 'OFFER.EDIT.SURVEYOR',
                emptyTranslationKey: 'OFFER.EDIT.NO_SURVEYOR'
            },
            additionalSurveyor: <ICompanyContactEditControlSchema>{
                formName: 'additionalSurveyorForm',
                controlId: 'additionalSurveyor',
                translationKey: 'OFFER.EDIT.SURVEYOR',
                emptyTranslationKey: 'OFFER.EDIT.NO_SURVEYOR'
            },
            status : <Attributes.IEnumItemEditControlSchema>{
                formName : "offerStatusControlForm",
                controlId : "offer-status",
                translationKey : "OFFER.EDIT.STATUS",
                fieldName : "statusId",
                enumTypeCode : Dto.EnumTypeCode.OfferStatus
            },
            price : <Attributes.IPriceEditControlSchema>{
                formName : "offerPriceControlForm",
                controlId : "offer-price",
                translationKey : "OFFER.EDIT.OFFER",
                fieldName : "price"
            },
            pricePerWeek : <Attributes.IPriceEditControlSchema>{
                formName : "offerPricePerWeekControlForm",
                controlId : "offer-price-per-week",
                translationKey : "OFFER.EDIT.OFFER",
                fieldName : "pricePerWeek",
                suffix : "OFFER.EDIT.OFFER_PER_WEEK"
            },
            offerDate : <Attributes.IDateEditControlSchema>{
                formName : "offerDateControlForm",
                controlId : "offer-date",
                translationKey : "OFFER.EDIT.OFFER_DATE",
                fieldName : "offerDate"
            },
            exchangeDate : <Attributes.IDateEditControlSchema>{
                formName : "exchangeDateControlForm",
                controlId : "offer-exchange-date",
                translationKey : "OFFER.EDIT.EXCHANGE_DATE",
                fieldName : "exchangeDate"
            },
            completionDate : <Attributes.IDateEditControlSchema>{
                formName : "completionDateControlForm",
                controlId : "offer-completion-date",
                translationKey : "OFFER.EDIT.COMPLETION_DATE",
                fieldName : "completionDate"
            },
            specialConditions : <Attributes.ITextEditControlSchema>{
                formName : "specialConfitionsControlForm",
                controlId : "offer-special-conditions",
                translationKey : "OFFER.EDIT.SPECIAL_CONDITIONS",
                fieldName : "specialConditions"
            },
            mortgageLoanToValue: <Attributes.IPercentNumberControlSchema>{
                formName : "mortgageLoanToValueControlForm",
                controlId : "mortgage-loan-to-value",
                translationKey: "OFFER.EDIT.MORTGAGE_LOAN_TO_VALUE",
                fieldName : "mortgageLoanToValue"
            },
            mortgageStatus: <Attributes.IEnumItemEditControlSchema>{
                formName: "mortgageStatusControlForm",
                controlId: "offer-mortgage-status",
                translationKey: "OFFER.EDIT.MORTGAGE_STATUS",
                fieldName: "mortgageStatusId",
                enumTypeCode: Dto.EnumTypeCode.MortgageStatus
            },
            mortgageSurveyStatus: <Attributes.IEnumItemEditControlSchema>{
                formName: "mortgageSurveyStatusControlForm",
                controlId: "offer-mortgage-survey-status",
                translationKey: "OFFER.EDIT.MORTGAGE_SURVEY_STATUS",
                fieldName: "mortgageSurveyStatusId",
                enumTypeCode: Dto.EnumTypeCode.MortgageSurveyStatus
            },
            additionalSurveyStatus: <Attributes.IEnumItemEditControlSchema>{
                formName: "additionalSurveyStatusControlForm",
                controlId: "offer-additional-survey-status",
                translationKey: "OFFER.EDIT.ADDITIONAL_SURVEY_STATUS",
                fieldName: "additionalSurveyStatusId",
                enumTypeCode: Dto.EnumTypeCode.AdditionalSurveyStatus
            },
            searchStatus: <Attributes.IEnumItemEditControlSchema>{
                formName: "searchStatusControlForm",
                controlId: "offer-search-status",
                translationKey: "OFFER.EDIT.SEARCH_STATUS",
                fieldName: "searchStatusId",
                enumTypeCode: Dto.EnumTypeCode.SearchStatus
            },
            enquiries: <Attributes.IEnumItemEditControlSchema>{
                formName: "enquiriesControlForm",
                controlId: "offer-enquiries",
                translationKey: "OFFER.EDIT.ENQUIRIES",
                fieldName: "enquiriesId",
                enumTypeCode: Dto.EnumTypeCode.Enquiries
            },
            mortgageSurveyDate: <Attributes.IDateEditControlSchema>{
                formName: "mortgageSurveyDateControlForm",
                controlId: "offer-mortgage-survey-date",
                translationKey: "OFFER.EDIT.MORTGAGE_SURVEY_DATE",
                fieldName: "mortgageSurveyDate"
            },
            additionalSurveyDate: <Attributes.IDateEditControlSchema>{
                formName: "additionalSurveyDateControlForm",
                controlId: "offer-additional-survey-date",
                translationKey: "OFFER.EDIT.ADDITIONAL_SURVEY_DATE",
                fieldName: "additionalSurveyDate"
            },
            progressComment: <Attributes.ITextEditControlSchema>{
                formName: "progressCommentControlForm",
                controlId: "offer-progress-comment",
                translationKey: "OFFER.EDIT.COMMENT",
                fieldName: "progressComment"
            },
            contractApproved: <Attributes.IRadioButtonsEditControlSchema>{
                formName: "offerContractApprovedControlForm",
                fieldName: "offerContractApproved",
                translationKey: "OFFER.EDIT.CONTRACT_APPROVED",
                radioButtons: [
                    { value: true, translationKey: "COMMON.YES" },
                    { value: false, translationKey: "COMMON.NO" }]
            }
        };

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private enumService: Services.EnumService,
            private $state: ng.ui.IStateService,
            private $window: ng.IWindowService,
            private $q: ng.IQService,
            private $scope: ng.IScope,
            private kfMessageService: Services.KfMessageService,
            private latestViewsProvider: LatestViewsProvider,
            private pubSub: PubSub,
            private eventAggregator: Core.EventAggregator,
            private appConfig: Common.Models.IAppConfig,
            private configService: Services.ConfigService,
            private offerService: Services.OfferService) {

            super(componentRegistry, $scope);
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
            eventAggregator.with(this).subscribe(CloseSidePanelEvent, () =>{
                this.hidePanels();
            });
            eventAggregator.with(this).subscribe(OpenCompanyContactEditPanelEvent, this.openCompanyContactEditPanel);

            this.offerOriginal = angular.copy(this.offer);

            this.createCompanyContacts();
        }

        private createCompanyContacts() {
            if (this.offer.broker) {
                this.brokerCompanyContact = new CompanyContactConnection(this.offer.broker, this.offer.brokerCompany);
            }
            if (this.offer.lender) {
                this.lenderCompanyContact = new CompanyContactConnection(this.offer.lender, this.offer.lenderCompany);
            }
            if (this.offer.surveyor) {
                this.surveyorCompanyContact = new CompanyContactConnection(this.offer.surveyor, this.offer.surveyorCompany);
            }
            if (this.offer.additionalSurveyor) {
                this.additionalSurveyorCompanyContact = new CompanyContactConnection(this.offer.additionalSurveyor, this.offer.additionalSurveyorCompany);
            }
        }

        isMortgageDetailsSectionVisible = () => {
            return this.config.offer_Broker
                || this.config.offer_Lender
                || this.config.offer_MortgageSurveyDate
                || this.config.offer_Surveyor;
        }

        isProgressSummarySectionVisible = () => {
            return this.config.offer_MortgageStatus
                || this.config.offer_MortgageSurveyStatus
                || this.config.offer_SearchStatus
                || this.config.offer_Enquiries
                || this.config.offer_ContractApproved;
        }

        isAdditionalSurveySectionVisible = () => {
            return this.config.offer_AdditionalSurveyor
                || this.config.offer_AdditionalSurveyDate;
        }
        
        onOldPanelsHidden = () =>{
            this.hideNewPanels();
        }

        hideNewPanels = () => {

        isOtherDetailsSectionVisible = () => {
            return this.config.offer_ProgressComment;
        }

        isProgressAndMortgageSectionVisible = () =>{
            return this.isMortgageDetailsSectionVisible()
                || this.isAdditionalSurveySectionVisible()
                || this.isProgressSummarySectionVisible()
                || this.isOtherDetailsSectionVisible();
        }

        companyContactPanelClosed = () => {
            this.isCompanyContactAddPanelVisible = Enums.SidePanelState.Closed;
            this.isBrokerEditPanelVisible = Enums.SidePanelState.Closed;
            this.isLenderEditPanelVisible = Enums.SidePanelState.Closed;
            this.isSurveyorEditPanelVisible = Enums.SidePanelState.Closed;
            this.isAdditionalSurveyorEditPanelVisible = Enums.SidePanelState.Closed;
        }

        openCompanyContactEditPanel = (event: OpenCompanyContactEditPanelEvent) => {
            this.hidePanels();

            switch (event.type) {
                case CompanyContactType.Broker:
                    this.isBrokerEditPanelVisible = Enums.SidePanelState.Opened;
                    break;
                case CompanyContactType.Lender:
                    this.isLenderEditPanelVisible = Enums.SidePanelState.Opened;
                    break;
                case CompanyContactType.Surveyor:
                    this.isSurveyorEditPanelVisible = Enums.SidePanelState.Opened;
                    break;
                case CompanyContactType.AdditionalSurveyor:
                    this.isAdditionalSurveyorEditPanelVisible = Enums.SidePanelState.Opened;
                    break;
            }
        }
        
        navigateToActivity = (activity: Business.Activity) => {
            var activityViewUrl = this.appConfig.appRootUrl + this.$state.href('app.activity-view', { id: activity.id }, { absolute: false });
            this.$window.open(activityViewUrl, '_blank');
        }

        navigateToRequirement = (requirement: Business.Requirement) => {
            var requirementViewUrl = this.appConfig.appRootUrl + this.$state.href('app.requirement-view', { id: requirement.id }, { absolute: false });
            this.$window.open(requirementViewUrl, '_blank');
        }

        // TODO: refactor activity preview panel to new one
        showActivityPreview = (offer: Common.Models.Business.Offer) =>{
            this.hidePanels();
            this.showPanel(this.components.panels.activityPreviewPanel);

            this.latestViewsProvider.addView({
                entityId: offer.activity.id,
                entityType: EntityType.Activity
            });
        }

        openMortgageSurveyDate() {
            this.mortgageSurveyDateOpen = true;
        }

        openAdditionalSurveyDate() {
            this.additionalSurveyDateOpen = true;
        }

        openExchangeDate() {
            this.exchangeDateOpen = true;
        }

        openCompletionDate() {
            this.completionDateOpen = true;
        }

        isDataValid = (): boolean => {
            var form = this.editOfferForm;
            form.$setSubmitted();
            return form.$valid;
        }

        onEnumLoaded = (result: any) => {
            this.offerStatuses = result[Dto.EnumTypeCode.OfferStatus];
            this.mortgageStatuses = result[Dto.EnumTypeCode.MortgageStatus];
            this.mortgageSurveyStatuses = result[Dto.EnumTypeCode.MortgageSurveyStatus];
            this.additionalSurveyStatuses = result[Dto.EnumTypeCode.AdditionalSurveyStatus];
            this.searchStatuses = result[Dto.EnumTypeCode.SearchStatus];
            this.enquiriesStatuses = result[Dto.EnumTypeCode.Enquiries];
            this.setDefaultStatuses();
        }

        offerAccepted = (): boolean => {
            var selectedOfferStatus: any = _.find(this.offerStatuses, (status: any) => status.id === this.offer.statusId);
            if (selectedOfferStatus) {
                return selectedOfferStatus.code === Common.Models.Enums.OfferStatus[Common.Models.Enums.OfferStatus.Accepted];
            }

            return false;
        }

        setDefaultStatuses = () => {
            var defaultMortgageStatus: any = _.find(this.mortgageStatuses, (status: any) => status.code === "Unknown");
            if (!this.offer.mortgageStatusId && defaultMortgageStatus) {
                this.offer.mortgageStatusId = defaultMortgageStatus.id;
            }

            var defaultMortgageSurveyStatus: any = _.find(this.mortgageSurveyStatuses, (status: any) => status.code === "Unknown");
            if (!this.offer.mortgageSurveyStatusId && defaultMortgageSurveyStatus) {
                this.offer.mortgageSurveyStatusId = defaultMortgageSurveyStatus.id;
            }

            var defaultAdditionalSurveyStatus: any = _.find(this.additionalSurveyStatuses, (status: any) => status.code === "Unknown");
            if (!this.offer.additionalSurveyStatusId && defaultAdditionalSurveyStatus) {
                this.offer.additionalSurveyStatusId = defaultAdditionalSurveyStatus.id;
            }

            var defaultSearchStatus: any = _.find(this.searchStatuses, (status: any) => status.code === "NotStarted");
            if (!this.offer.searchStatusId && defaultSearchStatus) {
                this.offer.searchStatusId = defaultSearchStatus.id;
            }

            var defaultEnquiriesStatus: any = _.find(this.enquiriesStatuses, (status: any) => status.code === "NotStarted");
            if (!this.offer.enquiriesId && defaultEnquiriesStatus) {
                this.offer.enquiriesId = defaultEnquiriesStatus.id;
            }
        }

        setCompanyContactData = () =>{
            this.offer.brokerId = this.brokerCompanyContact && this.brokerCompanyContact.contact.id;
            this.offer.brokerCompanyId = this.brokerCompanyContact && this.brokerCompanyContact.company.id; 

            this.offer.lenderId = this.lenderCompanyContact && this.lenderCompanyContact.contact.id;
            this.offer.lenderCompanyId = this.lenderCompanyContact && this.lenderCompanyContact.company.id; 

            this.offer.surveyorId = this.surveyorCompanyContact && this.surveyorCompanyContact.contact.id;
            this.offer.surveyorCompanyId = this.surveyorCompanyContact && this.surveyorCompanyContact.company.id; 

            this.offer.additionalSurveyorId = this.additionalSurveyorCompanyContact && this.additionalSurveyorCompanyContact.contact.id;
            this.offer.additionalSurveyorCompanyId = this.additionalSurveyorCompanyContact && this.additionalSurveyorCompanyContact.company.id; 
        }

        save() {
            this.setCompanyContactData();

            this.offer.offerDate = Core.DateTimeUtils.createDateAsUtc(this.offer.offerDate);
            this.offer.exchangeDate = Core.DateTimeUtils.createDateAsUtc(this.offer.exchangeDate);
            this.offer.completionDate = Core.DateTimeUtils.createDateAsUtc(this.offer.completionDate);
            this.offer.mortgageSurveyDate = Core.DateTimeUtils.createDateAsUtc(this.offer.mortgageSurveyDate);
            this.offer.additionalSurveyDate = Core.DateTimeUtils.createDateAsUtc(this.offer.additionalSurveyDate);

            this.offerService.updateOffer(this.offer)
                .then((offer: Dto.IOffer) => {
                    this.$state
                        .go('app.offer-view', offer)
                        .then(() => this.kfMessageService.showSuccessByCode('OFFER.EDIT.OFFER_EDIT_SUCCESS'));
                }, (response: any) => {
                    this.kfMessageService.showErrors(response);
                });
        }

        cancel() {
            this.$state.go('app.offer-view', { id: this.offer.id });
        }

        defineComponentIds() {
            this.componentIds = {
                activityPreviewSidePanelId: 'editOffer:activityPreviewSidePanelComponent'
            };
        }

        // TODO: refactor activity preview panel to new one
        defineComponents() {
            this.components = {
                panels: {
                    activityPreviewPanel: () => { return this.componentRegistry.get(this.componentIds.activityPreviewSidePanelId) }
                }
            };
        }

        offerStatusChanged = (id: string) =>{
            this.offer.statusId = id;
            this.reloadConfig();
        }

        reloadConfig = () => {
            this.configService.getOffer(Enums.PageTypeEnum.Update, this.offer.requirement.requirementTypeId, this.offer.offerTypeId, this.offer)
                .then((newConfig: IOfferEditConfig) => {
                    this.config = newConfig;
                });
        }
    }

    angular.module('app').controller('OfferEditController', OfferEditController);
};