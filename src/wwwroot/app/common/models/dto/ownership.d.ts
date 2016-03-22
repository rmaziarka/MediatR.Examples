declare module Antares.Common.Models.Dto {
    interface IOwnership {
        id: string;
        createDate: Date;
        purchaseDate: Date;
        sellingDate: Date;
        buyingPrice?: number;
        sellingPrice?: number;
        isCurrentOwner: boolean;
        contacts: IContact[];
    }
}