/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class DecimalDirective implements ng.IDirective {
        restrict = 'A';
        require = 'ngModel';
        // terminal and priority must be set because this attribute is dynamically added
        terminal = true;
        priority = 1000;
        link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: ng.INgModelController) => {

            ctrl.$parsers.unshift((viewValue: any) => {
                var floatRegEx: RegExp = /^\-?\d+((\.|\,)\d+)?$/;
                var conditionPassed = floatRegEx.test(viewValue);

                if (viewValue === "") {
                    return viewValue;
                }

                if (conditionPassed) {
                    ctrl.$setValidity('number', true);
                    if (typeof viewValue === "number") {
                        return viewValue;
                    }
                    else {
                        var floatWithDot: string = viewValue.replace(',', '.');
                        return parseFloat(floatWithDot);
                    }
                }
                else {

                    ctrl.$setValidity('number', false);
                    return undefined;
                }
            });
        };

        static factory() {
            var directive = () => {
                return new DecimalDirective();
            };

            directive['$inject'] = [];
            return directive;
        }

    }

    angular.module('app').directive('ddpDecimal', DecimalDirective.factory());
}
