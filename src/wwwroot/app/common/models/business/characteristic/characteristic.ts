/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {

    export class Characteristic implements Dto.ICharacteristic {
        // base implementation for ICharacteristic
        id: string;
        code: string;
        displayText: boolean;
        enabled: boolean;
        // extension for IChracteristicGroupItem
        order: number;

        constructor(characteristicGroupItem?: Dto.ICharacteristicGroupItem){
            if (characteristicGroupItem) {
                angular.extend(this, characteristicGroupItem.characteristic);

                this.order = characteristicGroupItem.order;
            }
        }
    }
}