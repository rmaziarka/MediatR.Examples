/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import IOwnership = Common.Models.Dto.IOwnership;
    import IContact = Common.Models.Dto.IContact;

    export class OwnershipAddController {
        static $inject = ['componentRegistry', 'dataAccessService', '$scope'];

        componentId: string;
        ownership: IOwnership = <IOwnership>{};
        ownershipTypes: any;
        datepickers: any = {
            purchaseOpened: false,
            sellOpened: false
        };

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $scope: ng.IScope) {

            this.ownershipTypes = dataAccessService.getEnumResource().get({ code: 'OwnershipType' });
            componentRegistry.register(this, this.componentId);
        }

        loadOwnership = (contacts: IContact[]) => {
            this.ownership.contacts = contacts;
        }

        getOwnership = () => {
            return this.ownership;
        }

        openPurchaseDate = () => {
            this.datepickers.purchaseOpened = true;
        }

        openSellDate = () => {
            this.datepickers.sellOpened = true;
        }

        isDataValid = (): boolean =>{
            var form = this.$scope["addOwnershipForm"];
            form.$setSubmitted();
            return form.$valid;
        }
    }

    angular.module('app').controller('OwnershipAddController', OwnershipAddController);
}