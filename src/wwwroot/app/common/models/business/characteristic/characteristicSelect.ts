/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CharacteristicSelect {
        characteristicId: string = '';
        text: string = '';
        isSelected: boolean = false;

        constructor(characteristic?: Dto.IPropertyCharacteristic) {
            if (characteristic) {
                this.characteristicId = characteristic.characteristicId;
                this.text = characteristic.text;
                this.isSelected = true;
            }
        }

        clear = () => {
            this.text = '';
            this.isSelected = false;
        }
    }
}