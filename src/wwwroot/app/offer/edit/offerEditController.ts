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

        isCompanyContactAddPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isBrokerEditPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isLenderEditPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isSurveyorEditPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;
        isAdditionalSurveyorEditPanelVisible: Enums.SidePanelState = Enums.SidePanelState.Untouched;

        contactToSelect: string = '';

        brokerCompanyContact: CompanyContactConnection = null;
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
            eventAggregator.with(this).subscribe(CloseSidePanelEvent, this.companyContactPanelClosed);
            eventAggregator.with(this).subscribe(OpenCompanyContactEditPanelEvent, this.openCompanyContactEditPanel);

            this.offerOriginal = angular.copy(this.offer);

            this.createCompanyContacts();
        }

        private createCompanyContacts() {
            if (this.offer.broker) {
                this.brokerCompanyContact = new CompanyContactConnection(this.offer.broker, this.offer.brokerCompany);
            }
            
        }

        companyContactPanelClosed = () => {
            this.isCompanyContactAddPanelVisible = Enums.SidePanelState.Closed;
            this.isBrokerEditPanelVisible = Enums.SidePanelState.Closed;
        }

        openCompanyContactEditPanel = (event: OpenCompanyContactEditPanelEvent) => {
            this.hidePanels();

            switch (event.type) {
                case CompanyContactType.Broker:
                    this.isBrokerEditPanelVisible = Enums.SidePanelState.Opened;
                    break;

                    // TODO: Add other company contact types
            }
        }

        showBrokerSelectPanel = () => {
            this.contactToSelect = 'Broker';
            this.sidePanelSelectedCompanyContacts = [new Business.CompanyContact(null, this.offer.broker, this.offer.brokerCompany)];

            this.hidePanels();
            this.isCompanyContactAddPanelVisible = Enums.SidePanelState.Opened;
        }

        showLenderSelectPanel = () => {
            this.contactToSelect = 'Lender';
            this.sidePanelSelectedCompanyContacts = [new Business.CompanyContact(null, this.offer.lender, this.offer.lenderCompany)];

            this.hidePanels();
            this.isCompanyContactAddPanelVisible = Enums.SidePanelState.Opened;
        }

        showSurveyorSelectPanel = () => {
            this.contactToSelect = 'Surveyor';
            this.sidePanelSelectedCompanyContacts = [new Business.CompanyContact(null, this.offer.surveyor, this.offer.surveyorCompany)];

            this.hidePanels();
            this.isCompanyContactAddPanelVisible = Enums.SidePanelState.Opened;
        }

        showAdditionalSurveyorSelectPanel = () => {
            this.contactToSelect = 'AadditionalSurveyor';
            this.sidePanelSelectedCompanyContacts = [new Business.CompanyContact(null, this.offer.additionalSurveyor, this.offer.additionalSurveyorCompany)];

            this.hidePanels();
            this.isCompanyContactAddPanelVisible = Enums.SidePanelState.Opened;
        }

        onContactSelected = (companyContact: Business.CompanyContact[]) => {
            switch (this.contactToSelect) {
                case 'Broker':
                    if (companyContact.length > 0) {
                        this.offer.broker = new Business.Contact(companyContact[0].contact, companyContact[0].company);
                        this.offer.brokerId = companyContact[0].contact.id;
                        this.offer.brokerCompany = companyContact[0].company;
                        this.offer.brokerCompanyId = companyContact[0].company.id;
                    }
                    else {
                        this.offer.broker = null;
                        this.offer.brokerId = null;
                        this.offer.brokerCompany = null;
                        this.offer.brokerCompanyId = null;
                    }
                    break;
                case 'Lender':
                    if (companyContact.length > 0) {
                        this.offer.lender = new Business.Contact(companyContact[0].contact, companyContact[0].company);
                        this.offer.lenderId = companyContact[0].contact.id;
                        this.offer.lenderCompany = companyContact[0].company;
                        this.offer.lenderCompanyId = companyContact[0].company.id;
                    }
                    else {
                        this.offer.lender = null;
                        this.offer.lenderId = null;
                        this.offer.lenderCompany = null;
                        this.offer.lenderCompanyId = null;
                    }
                    break;
                case 'Surveyor':
                    if (companyContact.length > 0) {
                        this.offer.surveyor = new Business.Contact(companyContact[0].contact, companyContact[0].company);
                        this.offer.surveyorId = companyContact[0].contact.id;
                        this.offer.surveyorCompany = companyContact[0].company;
                        this.offer.surveyorCompanyId = companyContact[0].company.id;
                    }
                    else {
                        this.offer.surveyor = null;
                        this.offer.surveyorId = null;
                        this.offer.surveyorCompany = null;
                        this.offer.surveyorCompanyId = null;
                    }
                    break;
                case 'AadditionalSurveyor':
                    if (companyContact.length > 0) {
                        this.offer.additionalSurveyor = new Business.Contact(companyContact[0].contact, companyContact[0].company);
                        this.offer.additionalSurveyorId = companyContact[0].contact.id;
                        this.offer.additionalSurveyorCompany = companyContact[0].company;
                        this.offer.additionalSurveyorCompanyId = companyContact[0].company.id;
                    }
                    else {
                        this.offer.additionalSurveyor = null;
                        this.offer.additionalSurveyorId = null;
                        this.offer.additionalSurveyorCompany = null;
                        this.offer.additionalSurveyorCompanyId = null;
                    }
                    break;
            }

            this.isCompanyContactAddPanelVisible = Enums.SidePanelState.Closed;
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
            this.isCompanyContactAddPanelVisible = Enums.SidePanelState.Closed;
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

        restoreOfferProgressSummary = () => {
            this.offer.mortgageStatusId = this.offerOriginal.mortgageStatusId;
            this.offer.mortgageSurveyStatusId = this.offerOriginal.mortgageSurveyStatusId;
            this.offer.additionalSurveyStatusId = this.offerOriginal.additionalSurveyStatusId;
            this.offer.searchStatusId = this.offerOriginal.searchStatusId;
            this.offer.enquiriesId = this.offerOriginal.enquiriesId;
            this.offer.contractApproved = this.offerOriginal.contractApproved;

            this.offer.mortgageLoanToValue = this.offerOriginal.mortgageLoanToValue;

            this.offer.brokerId = this.offerOriginal.brokerId;
            this.offer.brokerCompanyId = this.offerOriginal.brokerCompanyId;

            this.offer.lenderId = this.offerOriginal.lenderId;
            this.offer.lenderCompanyId = this.offerOriginal.lenderCompanyId;

            this.offer.mortgageSurveyDate = this.offerOriginal.mortgageSurveyDate;

            this.offer.surveyorId = this.offerOriginal.surveyorId;
            this.offer.surveyorCompanyId = this.offerOriginal.surveyorCompanyId;

            this.offer.additionalSurveyDate = this.offerOriginal.additionalSurveyDate;

            this.offer.additionalSurveyorId = this.offerOriginal.additionalSurveyorId;
            this.offer.additionalSurveyorCompanyId = this.offerOriginal.additionalSurveyorCompanyId;

            this.offer.progressComment = this.offerOriginal.progressComment;
        }

        save() {
            if (this.offerAccepted() === false) {
                this.restoreOfferProgressSummary();
            }

            this.offer.brokerId = this.brokerCompanyContact && this.brokerCompanyContact.contact.id;
            this.offer.brokerCompanyId = this.brokerCompanyContact && this.brokerCompanyContact.company.id;

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

        offerStatusChanged = () => {
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