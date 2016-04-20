/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Common.Models.Business;

    export class RangeAttributeController {
        public minValue: number;
        public maxValue: number;
        public attribute: Business.Attribute;
        mode: string = 'edit';

        getTemplateUrl(){
            return 'app/common/components/attribute/range/' + this.mode + 'RangeAttribute.html';
        }
    }

    angular.module('app').controller('rangeAttributeController', RangeAttributeController);
}