/// <reference path="../../../typings/_all.d.ts" />

module Antares.Requirement.View {
    export class RangeViewController {
        isMinDefined: boolean = false;
        isMaxDefined: boolean = false;
        label: any;
        min: number;
        max: number;
        suffix: any;
        isVisible: boolean = true;

        constructor() {
            this.isMinDefined = this.min !== undefined && angular.isNumber(this.min);
            this.isMaxDefined = this.max !== undefined && angular.isNumber(this.max);
        }
    }

    angular.module('app').controller('rangeViewController', RangeViewController);
}