/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {

    export class CharacteristicGroup implements Dto.ICharacteristicGroup {
        // base implementation for ICharacteristicGroup
        id: string;
        code: string;
        // extension for ICharacteristicGroupUsage
        propertyTypeId: string;
        countryId: string;
        chracteristicGroupItems: Characteristic[];
        order: number;
        displayLabel: boolean;

        constructor(characteristicGroupUsage?: Dto.ICharacteristicGroupUsage){
            if (characteristicGroupUsage) {
                angular.extend(this, characteristicGroupUsage.characteristicGroup);

                this.propertyTypeId = characteristicGroupUsage.propertyTypeId;
                this.countryId = characteristicGroupUsage.countryId;
                this.order = characteristicGroupUsage.order;
                this.displayLabel = characteristicGroupUsage.displayLabel;

                this.chracteristicGroupItems = characteristicGroupUsage.chracteristicGroupItems.map((characteristicGroupItem: Dto.ICharacteristicGroupItem) => { return new Characteristic(characteristicGroupItem) });
            }
        }
    }
}