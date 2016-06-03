/// <reference path="../../typings/_all.d.ts" />

module Antares.Offer {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class OfferEditController {
        public offer: Business.Offer;

        selectedStatus: any;
        statuses: any;
        editOfferForm: any;

        offerDateOpen: boolean = false;
        exchangeDateOpen: boolean = false;
        completionDateOpen: boolean = false;

        constructor(
            private dataAccessService: Services.DataAccessService,
            private enumService: Services.EnumService,
            private $state: ng.ui.IStateService,
            private $q: ng.IQService,
            private kfMessageService: Services.KfMessageService) {
            this.enumService.getEnumPromise().then(this.onEnumLoaded);
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

        save(){
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
    }

    angular.module('app').controller('OfferEditController', OfferEditController);
};