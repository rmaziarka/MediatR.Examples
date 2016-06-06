/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;
    import LatestViewsProvider = Providers.LatestViewsProvider;
    import EntityType = Common.Models.Enums.EntityTypeEnum;

    export class OfferEditController extends Core.WithPanelsBaseController {
        public offer: Business.Offer;

        selectedStatus: any;
        statuses: any;
        editOfferForm: any;

        offerDateOpen: boolean = false;
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
            this.statuses = result[Dto.EnumTypeCode.OfferStatus];
            this.selectedStatus = _.find(this.statuses, (status: any) => status.id === this.offer.statusId);
        }

        save() {
            if (!this.isDataValid()) {
                return this.$q.reject();
            }

            var offerResource = this.dataAccessService.getOfferResource();
            this.offer.statusId = this.selectedStatus.id;
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