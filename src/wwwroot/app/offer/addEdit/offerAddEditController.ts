///<reference path="../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        import Dto = Common.Models.Dto;
        import Business = Common.Models.Business;
        declare var moment: any;

        export class OfferAddEditController {
            componentId: string;

            offerDateOpen: boolean = false;
            exchangeDateOpen: boolean = false;
            completionDateOpen: boolean = false;
            today: Date = new Date();

            defaultStatusCode: string = 'New';
            statuses: any;
            selectedStatus: any;

            offer: Business.Offer;
            activity: Dto.IActivity = <Dto.IActivity>{};
            requirement: Dto.IRequirement;

            addOfferForm: any;

            mode: string;

            originalOffer: Business.Offer;

            constructor(
                componentRegistry: Antares.Core.Service.ComponentRegistry,
                private dataAccessService: Antares.Services.DataAccessService,
                private enumService: Services.EnumService,
                private $state: ng.ui.IStateService,
                private $q: ng.IQService) {

                componentRegistry.register(this, this.componentId);
                this.enumService.getEnumPromise().then(this.onEnumLoaded);
                this.requirement = <Dto.IRequirement>{};
            }

            getOriginalOffer = (): Business.Offer => {
                return this.originalOffer;
            }

            setOffer = (offer: Business.Offer) => {
                this.originalOffer = offer;
                this.offer = angular.copy(offer);
                this.activity = offer.activity;
                this.selectedStatus = _.find(this.statuses, (status: any) => status.id === this.offer.statusId);
            }

            reset = () => {
                this.offer = new Business.Offer(<Dto.IOffer>{
                    offerDate: new Date(),
                    activityId: this.activity.id,
                    requirementId: this.requirement.id
                });

                this.setDefaultOfferStatus();
                this.addOfferForm.$setPristine();
                this.addOfferForm.$setUntouched();
            }

            onEnumLoaded = (result: any) => {
                this.statuses = result[Dto.EnumTypeCode.OfferStatus];
                this.setDefaultOfferStatus();
            }

            setDefaultOfferStatus = () => {
                this.selectedStatus = null;
                if (this.statuses) {
                    var defaultActivityStatus: any = _.find(this.statuses, { 'code': this.defaultStatusCode });
                    if (defaultActivityStatus) {
                        this.selectedStatus = defaultActivityStatus;
                    }
                }
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
                this.addOfferForm.$setSubmitted();
                return this.addOfferForm.$valid;
            }

            saveOffer = () => {
                if (!this.isDataValid()) {
                    return this.$q.reject();
                }

                var offerResource = this.dataAccessService.getOfferResource();
                this.offer.statusId = this.selectedStatus.id;
                this.offer.offerDate = Core.DateTimeUtils.createDateAsUtc(this.offer.offerDate);
                this.offer.exchangeDate = Core.DateTimeUtils.createDateAsUtc(this.offer.exchangeDate);
                this.offer.completionDate = Core.DateTimeUtils.createDateAsUtc(this.offer.completionDate);

                if (this.mode === "add") {
                    this.offer.statusId = this.selectedStatus.id;

                    return offerResource
                        .save(this.offer)
                        .$promise;
                }
                else if (this.mode === "edit") {

                    var updateOffer: Dto.IOffer = angular.copy(this.offer);
                    return offerResource
                        .update(updateOffer)
                        .$promise;
                }
            }

            goToActivityView = () => {
                this.$state.go('app.activity-view', { id: this.activity.id });
            }
        }
        angular.module('app').controller('offerAddEditController', OfferAddEditController);
    }
}