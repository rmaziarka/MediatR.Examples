/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class NumberPrecisionDirective implements ng.IDirective {
        restrict = 'A';
        require = 'ngModel';
        link: any;

        constructor() {
            this.link = this.unboundLink.bind(this);
        }

        unboundLink(scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ngModel: ng.INgModelController) {
            ngModel.$validators['numberPrecision'] = (modelValue: string) => {
                if (!modelValue) {
                    return true;
                }
                
                var precision: number = parseInt(<string>attrs["numberPrecision"]);
                var floatRegEx: RegExp = precision > 0 ? new RegExp("^\\-?\\d+([\\.\\,]\\d{1," + precision + "})?$"): /^\-?\d+$/;
                return floatRegEx.test(modelValue);
            }
        }

        static factory() {
            var directive = () => {
                return new NumberPrecisionDirective();
            };

            directive['$inject'] = [];
            return directive;
        }
    }

    angular.module('app').directive('numberPrecision', NumberPrecisionDirective.factory());
}