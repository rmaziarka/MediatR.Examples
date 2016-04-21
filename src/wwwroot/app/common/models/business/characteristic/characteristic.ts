/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {

    export class Characteristic {
        id: string;
        isDisplayText: boolean;
        isEnabled: boolean;

        constructor(characteristicGroupItem?: Dto.ICharacteristicGroupItem){
            if (characteristicGroupItem) {
                angular.extend(this, characteristicGroupItem.characteristic);
            }
        }
    }
}