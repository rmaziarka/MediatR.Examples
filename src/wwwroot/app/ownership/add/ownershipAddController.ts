/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import IOwnership = Antares.Common.Models.Dto.IOwnership;
    import IContact = Antares.Common.Models.Dto.IContact;

    export class OwnershipAddController {
        static $inject = ['componentRegistry', 'dataAccessService'];

        componentId: string;
        ownership: IOwnership = <IOwnership>{};
        ownershipTypes: any;
        datepickers: any = {
            purchaseOpened: false,
            sellOpened: false
        }

        constructor(
            componentRegistry: Antares.Core.Service.ComponentRegistry,
            private dataAccessService: Antares.Services.DataAccessService) {

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
    }

    angular.module('app').controller('OwnershipAddController', OwnershipAddController);
}