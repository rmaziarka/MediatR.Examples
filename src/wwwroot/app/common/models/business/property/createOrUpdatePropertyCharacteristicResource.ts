/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Models.Business {
    export class CreateOrUpdatePropertyCharacteristicResource {
        characteristicId: string = '';
        text: string = '';

        constructor(characteristicSelect: Business.CharacteristicSelect){
            this.characteristicId = characteristicSelect.characteristicId;
            this.text = characteristicSelect.text;
        }
    }
}