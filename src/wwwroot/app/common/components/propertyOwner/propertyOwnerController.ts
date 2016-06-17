/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    'use strict';

    import Dto = Models.Dto;
    import Business = Models.Business;
    import Enums = Models.Enums;

    export class PropertyOwnerController {
        ownerships: Business.PropertySearchResultOwnership[];

        public currentOwnershipTypeId: string = null;
        public currentContacts: Business.PropertySearchResultContact[] = [];

        constructor(private enumService: Services.EnumService) {
            if (this.ownerships != null && this.ownerships.length > 0) {
                this.enumService.getEnumPromise()
                    .then(this.onLoadedEnums);
            }
        }

        private onLoadedEnums = (result: any) => {

            var ownershipTypes: Dto.IEnumItem[] = result[Dto.EnumTypeCode.OwnershipType];

            var ownershipType: Dto.IEnumItem = null;
            var ownershipContacts: Business.PropertySearchResultContact[] = [];

            ownershipType = this.selectEnumItem(ownershipTypes, Enums.OwnershipTypeEnum.Leaseholder);
            ownershipContacts = this.selectCurrentOwnershipContacts(this.ownerships, ownershipType);

            if (ownershipContacts.length == 0) {
                ownershipType = this.selectEnumItem(ownershipTypes, Enums.OwnershipTypeEnum.Freeholder);
                ownershipContacts = this.selectCurrentOwnershipContacts(this.ownerships, ownershipType);
            }

            this.currentOwnershipTypeId = ownershipType.id;
            this.currentContacts = ownershipContacts;
        }

        private selectEnumItem = (ownershipTypes: Dto.IEnumItem[], ownershipTypeCode: Enums.OwnershipTypeEnum): Dto.IEnumItem => {
            return <Dto.IEnumItem>_.find(ownershipTypes, { 'code': ownershipTypeCode });
        }

        private selectCurrentOwnershipContacts = (ownerships: Business.PropertySearchResultOwnership[], ownershipType: Dto.IEnumItem): Business.PropertySearchResultContact[] => {
            var result: Business.PropertySearchResultContact[] = [];
            for (var i = 0; i < this.ownerships.length; i++) {
                var ownership = this.ownerships[i];
                if (ownership.isCurrentOwner && ownership.ownershipTypeId.toLowerCase() == ownershipType.id) {
                    result = result.concat(ownership.contacts);
                }
            }
            return result;
        }

        public isShow = (): boolean => {
            return this.currentOwnershipTypeId && this.currentContacts.length > 0;
        }

    }

    angular.module('app').controller('propertyOwnerController', PropertyOwnerController);
}