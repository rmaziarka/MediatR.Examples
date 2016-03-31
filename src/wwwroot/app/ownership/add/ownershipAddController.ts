/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;

    export class OwnershipAddController {
        static $inject = ['componentRegistry', 'dataAccessService', '$scope', '$q'];

        componentId: string;
        ownership: Dto.IOwnership = new Dto.Ownership();
        ownerships: Dto.IOwnership[];
        ownershipTypes: any;
        datepickers: any = {
            purchaseOpened: false,
            sellOpened: false
        };

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $scope: ng.IScope,
            private $q: ng.IQService) {

            this.ownershipTypes = dataAccessService.getEnumResource().get({ code: 'OwnershipType' });
            componentRegistry.register(this, this.componentId);
        }

        saveOwnership = (propertyId: string): ng.IPromise<void> =>{

            if (!this.isDataValid()) {
                return this.$q.reject();
            }

            var createOwnershipCommand = this.getCreateOwnershipCommand();

            var propertyResource = this.dataAccessService.getPropertyResource();

            return propertyResource
                .createOwnership({ propertyId: propertyId }, createOwnershipCommand)
                .$promise
                .then((ownership: Antares.Common.Models.Dto.IOwnership) =>{
                    this.ownerships.push(ownership);
                });
        }

        getCreateOwnershipCommand() {
            var ownership: any = angular.copy(this.ownership);
            ownership.contactIds = ownership.contacts.map((item: Dto.IContact) => { return item.id; });
            ownership.ownershipTypeId = ownership.ownershipType.id;
            delete ownership.contacts;
            delete ownership.ownershipType;

            return ownership;
        }

        loadOwnership = (contacts: Dto.IContact[]) => {
            this.ownership.contacts = contacts;
        }

        clearOwnership = () => {
            this.ownership = new Dto.Ownership();
            this.ownership.purchaseDate = null;
            this.ownership.sellDate = null;
            var form = this.$scope["addOwnershipForm"];
            form.$setPristine();
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