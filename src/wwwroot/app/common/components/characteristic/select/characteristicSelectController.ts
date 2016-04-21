/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Common.Models.Business;

    export class CharacteristicSelectController {
        public characteristic: Business.Characteristic;
        public characteristicSelect: Business.CharacteristicSelect;
    }

    angular.module('app').controller('CharacteristicSelectController', CharacteristicSelectController);
}