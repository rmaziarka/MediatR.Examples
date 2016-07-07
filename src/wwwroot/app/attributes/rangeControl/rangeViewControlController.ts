/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {

    export class RangeViewControlController {
        // bindings 
        public min: number;
        public max: number;
        public config: any;
        public schema: ITextEditControlSchema;

        get showMaxPrefix() {
            return this.min == undefined && angular.isNumber(this.max);
        };
        get showMinPrefix() {
            return this.max == undefined && angular.isNumber(this.min);
        }
        get showHyphen() {
            return (angular.isNumber(this.min) && angular.isNumber(this.max) && this.min !== this.max) || (this.min == undefined && this.max == undefined);
        }
    }

    angular.module('app').controller('RangeViewControlController', RangeViewControlController);
};