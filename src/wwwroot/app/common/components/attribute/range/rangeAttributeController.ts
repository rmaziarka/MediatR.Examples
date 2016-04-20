/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    import Business = Common.Models.Business;

    export class RangeAttributeController {
        public minValue: number;
        public maxValue: number;
        public attribute: Business.Attribute;
        
        get showMaxPrefix(){
            return this.minValue == undefined && angular.isNumber(this.maxValue);
        };
        get showMinPrefix(){
            return this.maxValue == undefined && angular.isNumber(this.minValue);
        }
        get showHyphen(){
            return (angular.isNumber(this.minValue) && angular.isNumber(this.maxValue) && this.minValue !== this.maxValue) || (this.minValue == undefined && this.maxValue == undefined);
        }

        mode: string = 'edit';

        getTemplateUrl(){
            return 'app/common/components/attribute/range/' + this.mode + 'RangeAttribute.html';
        }
    }

    angular.module('app').controller('rangeAttributeController', RangeAttributeController);
}