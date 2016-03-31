module Antares.Common.Models.Dto {
    export class Ownership implements IOwnership {
        id: string;
        createDate: Date;
        purchaseDate: Date;
        sellDate: Date;
        buyPrice: number;
        sellPrice: number;
        isCurrentOwner: boolean;
        ownershipType: number;
        contacts: IContact[];

        constructor(){
            this.createDate = null;
            this.purchaseDate = null;
            this.sellDate = null;
            this.buyPrice = null;
            this.sellPrice = null;
            this.isCurrentOwner = null;
            this.ownershipType = null;
            this.contacts = [];
        }
    }
}