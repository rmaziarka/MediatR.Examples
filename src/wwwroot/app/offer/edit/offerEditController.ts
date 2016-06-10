/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class OfferEditController extends Core.WithPanelsBaseController {
        public offer: Business.Offer;

        public enumTypeMortgageStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.MortgageStatus;
        public enumTypeMortgageSurveyStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.MortgageSurveyStatus;
        public enumTypeAdditionalSurveyStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.AdditionalSurveyStatus;
        public enumTypeSearchStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.SearchStatus;
        public enumTypeEnquiriesStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.Enquiries;
        public enumTypeOfferStatus: Dto.EnumTypeCode = Dto.EnumTypeCode.OfferStatus;

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

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService,
            private $state: ng.ui.IStateService,
            private $window: ng.IWindowService,
            private $q: ng.IQService,
            private $scope: ng.IScope,
            private kfMessageService: Services.KfMessageService,
            private latestViewsProvider: LatestViewsProvider) {
            super(componentRegistry, $scope);
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
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
            this.setSelectedStatuses();
        }

        offerAccepted = (): boolean => {
            var selectedOfferStatus: any = _.find(this.offerStatuses, (status: any) => status.id === this.offer.statusId);
            if (selectedOfferStatus) {
                return selectedOfferStatus.code === "Accepted";
            }

            return false;
        }

        setSelectedStatuses = () => {
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

        save() {
            if (!this.isDataValid()) {
                return this.$q.reject();
            }

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