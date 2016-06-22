/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    import Dto = Common.Models.Dto;
    import Business = Common.Models.Business;

    export class OwnershipAddController {
        static $inject = ['componentRegistry', 'dataAccessService', '$scope', '$q','$filter'];
        maxAllowedDate:string = this.$filter('date')(new Date(),'dd-MM-yyyy');
        componentId: string;
        ownerships: Business.Ownership[];

        public enumTypeOwnershipType: Dto.EnumTypeCode = Dto.EnumTypeCode.OwnershipType;
        public ownership: Business.Ownership = new Business.Ownership();
        public datepickers: any = {
            purchaseOpened: false,
            sellOpened: false
        };

        constructor(
            componentRegistry: Core.Service.ComponentRegistry,
            private dataAccessService: Services.DataAccessService,
            private $scope: ng.IScope,
            private $q: ng.IQService,
            private $filter:any) {

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
                .then((ownership: Common.Models.Dto.IOwnership) =>{
                    var ownershipModel = new Business.Ownership(ownership);

                    ownershipModel.purchaseDate = Core.DateTimeUtils.convertDateToUtc(ownershipModel.purchaseDate);
                    ownershipModel.sellDate = Core.DateTimeUtils.convertDateToUtc(ownershipModel.sellDate);

                    var newOwnerships: Business.Ownership[] = null;
                    newOwnerships = angular.copy(this.ownerships);
                    newOwnerships.push(ownershipModel);

                    this.ownerships = newOwnerships;
                    var form = this.$scope["addOwnershipForm"];
                    form.$setPristine();
                });
        }

        getCreateOwnershipCommand() {
            var ownership: any = angular.copy(this.ownership);
            ownership.contactIds = ownership.contacts.map((item: Dto.IContact) => { return item.id; });
            ownership.purchaseDate = Core.DateTimeUtils.createDateAsUtc(ownership.purchaseDate);
            ownership.sellDate = Core.DateTimeUtils.createDateAsUtc(ownership.sellDate);

            delete ownership.contacts;
            delete ownership.ownershipType;

            return ownership;
        }

        loadOwnership = (contacts: Business.Contact[]) => {
            this.ownership.contacts = contacts;
        }

        clearOwnership = () => {
            this.ownership = new Business.Ownership();
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

        setCurrentOwner = (): void => {
            if (this.ownership.isCurrentOwner)
            {
                this.ownership.sellDate = null
                this.ownership.sellPrice = null
            }
        }
    }

    angular.module('app').controller('OwnershipAddController', OwnershipAddController);
}