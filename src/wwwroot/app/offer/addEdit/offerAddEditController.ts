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
            requirement: Dto.IRequirement = <Dto.IRequirement>{};

            addOfferForm: any;

            constructor(
                componentRegistry: Antares.Core.Service.ComponentRegistry,
                private dataAccessService: Antares.Services.DataAccessService,
                private enumService: Services.EnumService,
                private $state: ng.ui.IStateService,
                private $q: ng.IQService) {

                componentRegistry.register(this, this.componentId);
                this.enumService.getEnumPromise().then(this.onEnumLoaded);
            }

            setOffer = (offer: Business.Offer) =>{
                this.offer = offer;
            }

            reset = () =>{
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

            goToActivityView = () => {
                this.$state.go('app.activity-view', { id: this.activity.id });
            }
        }
        angular.module('app').controller('offerAddEditController', OfferAddEditController);
    }
}