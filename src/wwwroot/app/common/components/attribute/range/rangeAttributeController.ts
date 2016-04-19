/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Common.Models.Business;

    export class RangeAttributeController {
        public minValue: number;
        public maxValue: number;
        public attribute: Business.Attribute;
    }

    angular.module('app').controller('rangeAttributeController', RangeAttributeController);
}