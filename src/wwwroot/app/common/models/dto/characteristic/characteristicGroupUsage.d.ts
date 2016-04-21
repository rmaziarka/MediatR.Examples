declare module Antares.Common.Models.Dto {
    interface ICharacteristicGroupUsage {
        id: string;
        propertyTypeId: string;
        countryId: string;
        characteristicGroup: ICharacteristicGroup;
        chracteristicGroupItems: ICharacteristicGroupItem[];
        order: number;
        displayLabel: boolean;
    }
}