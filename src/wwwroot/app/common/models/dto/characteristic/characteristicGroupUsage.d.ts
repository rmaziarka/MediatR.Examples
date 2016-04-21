declare module Antares.Common.Models.Dto {
    interface ICharacteristicGroupUsage {
        id: string;
        propertyTypeId: string;
        countryId: string;
        characteristicGroupId: string;
        characteristicGroupItems: ICharacteristicGroupItem[];
        order: number;
        isDisplayLabel: boolean;
    }
}