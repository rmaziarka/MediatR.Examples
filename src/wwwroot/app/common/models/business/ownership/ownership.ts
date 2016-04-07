/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class Ownership implements Dto.IOwnership {
        id: string = null;
        createDate: Date = null;
        purchaseDate: Date = null;
        sellDate: Date = null;
        buyPrice: number = null;
        sellPrice: number;
        isCurrentOwner: boolean;
        ownershipTypeId: string;
        ownershipType: Dto.IOwnershipType;
        contacts: Contact[];

        constructor(ownership?: Dto.IOwnership) {
            if (ownership) {
                angular.extend(this, ownership);
                this.contacts = ownership.contacts.map((value: Dto.IContact) => { return new Contact(value); });
                this.isCurrentOwner = !this.sellDate;
            }
        }

        public isVendor(): boolean {
            return this.isCurrentOwner && this.ownershipType.code == Enums.OwnershipTypeEnum.Freeholder;
        }
    }
}