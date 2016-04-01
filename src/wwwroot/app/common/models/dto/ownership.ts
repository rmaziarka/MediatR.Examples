/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Models.Dto {
    export class Ownership implements IOwnership {
        id: string = null;
        createDate: Date = null;
        purchaseDate: Date = null;
        sellDate: Date = null;
        buyPrice: number = null;
        sellPrice: number;
        isCurrentOwner: boolean;
        ownershipType: IOwnershipType;
        contacts: Contact[];

        constructor(ownership?: IOwnership) {
            if (ownership) {
                angular.extend(this, ownership);
                this.contacts = ownership.contacts.map((value: IContact) => { return new Dto.Contact(value); });
                this.isCurrentOwner = !this.sellDate;
            }
        }

        public isVendor(): boolean {
            return this.isCurrentOwner && this.ownershipType.code == OwnershipTypeEnum.Freeholder;
        }
    }
}