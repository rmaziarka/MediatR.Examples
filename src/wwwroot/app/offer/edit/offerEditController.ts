/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;
    import PubSub = Core.PubSub;

    export class OfferEditController extends Core.WithPanelsBaseController {
        public offer: Business.Offer;
        public offerOriginal: Business.Offer;

        public enumTypeMortgageStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.MortgageStatus;
        public enumTypeMortgageSurveyStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.MortgageSurveyStatus;
        public enumTypeAdditionalSurveyStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.AdditionalSurveyStatus;
        public enumTypeSearchStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.SearchStatus;
        public enumTypeEnquiriesStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.Enquiries;
        public enumTypeOfferStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.OfferStatus;

        sidePanelSelectedCompanyContacts: Business.CompanyContact[];

        offerStatuses: any;
        mortgageStatuses: any;
        mortgageSurveyStatuses: any;
        additionalSurveyStatuses: any;
        searchStatuses: any;
        enquiriesStatuses: any;

        editOfferForm: any;

        offerDateOpen: boolean = false;
        mortgageSurveyDateOpen: boolean = false;
        additionalSurveyDateOpen: boolean = false;
        exchangeDateOpen: boolean = false;
        completionDateOpen: boolean = false;
        isCompanyContactAddPanelVisible: boolean = false;
        contactToSelect: string = '';

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService,
            private $state: ng.ui.IStateService,
            private $window: ng.IWindowService,
            private $q: ng.IQService,
            private $scope: ng.IScope,
            private kfMessageService: Services.KfMessageService,
            private latestViewsProvider: LatestViewsProvider,
            private pubSub: PubSub) {
            super(componentRegistry, $scope);
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
            pubSub.with(this)
                .subscribe(Common.Component.CloseSidePanelMessage, () => {
                    this.isCompanyContactAddPanelVisible = false;
                })

            this.offerOriginal = angular.copy(this.offer);
            pubSub.with(this)
                .subscribe(Common.Component.CloseSidePanelEvent, () => {
                    this.isCompanyContactAddPanelVisible = false;
                });
        }

        showBrokerSelectPanel = () => {
            this.contactToSelect = 'Broker';
            this.sidePanelSelectedCompanyContacts = [new Business.CompanyContact(null, this.offer.broker, this.offer.brokerCompany)];
            this.isCompanyContactAddPanelVisible = true;
        }

        showLenderSelectPanel = () => {
            this.contactToSelect = 'Lender';
            this.sidePanelSelectedCompanyContacts = [new Business.CompanyContact(null, this.offer.lender, this.offer.lenderCompany)];
            this.isCompanyContactAddPanelVisible = true;
        }

        showSurveyorSelectPanel = () => {
            this.contactToSelect = 'Surveyor';
            this.sidePanelSelectedCompanyContacts = [new Business.CompanyContact(null, this.offer.surveyor, this.offer.surveyorCompany)];
            this.isCompanyContactAddPanelVisible = true;
        }

        showAdditionalSurveyorSelectPanel = () => {
            this.contactToSelect = 'AadditionalSurveyor';
            this.sidePanelSelectedCompanyContacts = [new Business.CompanyContact(null, this.offer.additionalSurveyor, this.offer.additionalSurveyorCompany)];
            this.isCompanyContactAddPanelVisible = true;
        }

        onContactSelected = (companyContact: Business.CompanyContact[]) => {
            switch (this.contactToSelect) {
                case 'Broker':
                    if (companyContact.length > 0) {
                        this.offer.broker = new Business.Contact(companyContact[0].contact, companyContact[0].company);
                        this.offer.brokerId = companyContact[0].contact.id;
                        this.offer.brokerCompanyId = companyContact[0].company.id;
                    }
                    else {
                        this.offer.broker = null;
                        this.offer.brokerId = null;
                        this.offer.brokerCompanyId = null;
                    }
                    break;
                case 'Lender':
                    if (companyContact.length > 0) {
                        this.offer.lender = new Business.Contact(companyContact[0].contact, companyContact[0].company);
                        this.offer.lenderId = companyContact[0].contact.id;
                        this.offer.lenderCompanyId = companyContact[0].company.id;
                    }
                    else {
                        this.offer.lender = null;
                        this.offer.lenderId = null;
                        this.offer.lenderCompanyId = null;
                    }
                    break;
                case 'Surveyor':
                    if (companyContact.length > 0) {
                        this.offer.surveyor = new Business.Contact(companyContact[0].contact, companyContact[0].company);
                        this.offer.surveyorId = companyContact[0].contact.id;
                        this.offer.surveyorCompanyId = companyContact[0].company.id;
                    }
                    else {
                        this.offer.surveyor = null;
                        this.offer.surveyorId = null;
                        this.offer.surveyorCompanyId = null;
                    }
                    break;
                case 'AadditionalSurveyor':
                    if (companyContact.length > 0) {
                        this.offer.additionalSurveyor = new Business.Contact(companyContact[0].contact, companyContact[0].company);
                        this.offer.additionalSurveyorId = companyContact[0].contact.id;
                        this.offer.additionalSurveyorCompanyId = companyContact[0].company.id;
                    }
                    else {
                        this.offer.additionalSurveyor = null;
                        this.offer.additionalSurveyorId = null;
                        this.offer.additionalSurveyorCompanyId = null;
                    }
                    break;
            }

            this.isCompanyContactAddPanelVisible = false;
        }

        navigateToActivity = (ativity: Business.Activity) => {
            this.$window.open(this.$state.href('app.activity-view', { id: ativity.id }, { absolute: true }), '_blank');
        }

        navigateToRequirement = (requirement: Business.Requirement) => {
            this.$window.open(this.$state.href('app.requirement-view', { id: requirement.id }, { absolute: true }), '_blank');
        }

        showActivityPreview = (offer: Common.Models.Business.Offer) => {
            this.showPanel(this.components.activityPreview);

            this.latestViewsProvider.addView({
                entityId: offer.activity.id,
                entityType: EntityType.Activity
            });
        }

        openOfferDate() {
            this.offerDateOpen = true;
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
            if (!this.isDataValid()) {
                return this.$q.reject();
            }

            if (this.offerAccepted() === false) {
                this.restoreOfferProgressSummary();
            }

            this.offer.offerDate = Core.DateTimeUtils.createDateAsUtc(this.offer.offerDate);
            this.offer.exchangeDate = Core.DateTimeUtils.createDateAsUtc(this.offer.exchangeDate);
            this.offer.completionDate = Core.DateTimeUtils.createDateAsUtc(this.offer.completionDate);
            this.offer.mortgageSurveyDate = Core.DateTimeUtils.createDateAsUtc(this.offer.mortgageSurveyDate);
            this.offer.additionalSurveyDate = Core.DateTimeUtils.createDateAsUtc(this.offer.additionalSurveyDate);

            var offerResource = this.dataAccessService.getOfferResource();
            var updateOffer: Dto.IOffer = angular.copy(this.offer);

            return offerResource
                .update(updateOffer)
                .$promise
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

        defineComponents() {
            this.components = {
                activityPreview: () => { return this.componentRegistry.get(this.componentIds.activityPreviewSidePanelId); }
            };
        }
    }

    angular.module('app').controller('OfferEditController', OfferEditController);
};