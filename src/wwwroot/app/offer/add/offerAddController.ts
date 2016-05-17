///<reference path="../../typings/_all.d.ts"/>

module Antares {
    export module Component {
        import Dto = Common.Models.Dto;
        declare var moment: any;

        export class OfferAddController {
            componentId: string;

            offerDateOpen: boolean = false;
            exchangeDateOpen: boolean = false;
            completionDateOpen: boolean = false;
            today: Date = new Date();

            defaultStatusCode: string = 'New';
            statuses: any;
            selectedStatus: any;

            offer: Dto.IOffer;
            activity: Dto.IActivity;
            requirement: Dto.IRequirement;

            addOfferForm: any;

            constructor(
                componentRegistry: Antares.Core.Service.ComponentRegistry,
                private dataAccessService: Antares.Services.DataAccessService,
                private enumService: Services.EnumService,
                private $q: ng.IQService) {

                componentRegistry.register(this, this.componentId);
                this.enumService.getEnumPromise().then(this.onEnumLoaded);
            }

            reset = () =>{
                this.offer = <Dto.IOffer>{};
                if (this.activity) {
                    this.offer.activityId = this.activity.id;
                }

                if (this.requirement) {
                    this.offer.requirementId = this.requirement.id;
                }

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

            openOfferDate(){
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

            saveOffer() {
                if (!this.isDataValid()) {
                    return this.$q.reject();
                }

                this.offer.statusId = this.selectedStatus.id;

                var offerResource = this.dataAccessService.getOfferResource();
                return offerResource
                    .save(this.offer)
                    .$promise;
            }

        }
        angular.module('app').controller('offerAddController', OfferAddController);
    }
}