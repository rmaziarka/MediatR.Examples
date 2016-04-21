/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {

    export class CharacteristicGroup {
        id: string;
        propertyTypeId: string;
        countryId: string;
        order: number;
        isDisplayLabel: boolean;
        characteristicGroupItems: Characteristic[];

        constructor(characteristicGroupUsage?: Dto.ICharacteristicGroupUsage){
            if (characteristicGroupUsage) {
                this.id = characteristicGroupUsage.characteristicGroupId;
                this.propertyTypeId = characteristicGroupUsage.propertyTypeId;
                this.countryId = characteristicGroupUsage.countryId;
                this.order = characteristicGroupUsage.order;
                this.isDisplayLabel = characteristicGroupUsage.isDisplayLabel;

                this.characteristicGroupItems = characteristicGroupUsage.characteristicGroupItems.map((characteristicGroupItem: Dto.ICharacteristicGroupItem) => { return new Characteristic(characteristicGroupItem) });
            }
        }
    }
}