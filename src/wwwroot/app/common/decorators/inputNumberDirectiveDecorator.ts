/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Decorators {
    export class InputNumberDirectiveDecorator {
        public static decoratorFunction($delegate: ng.IDirective[]) {
            var directive: ng.IDirective = $delegate[0];
            var link: ng.IDirectivePrePost = directive.link;
            var baseLinkPre = link.pre;
            

            link.pre = function (scope: ng.IScope, el: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrls: ng.INgModelController[]) {
                    baseLinkPre.apply(this, arguments);

                    if (attrs['type'] === 'number') {
                        ctrls[0].$parsers.unshift(InputNumberDirectiveDecorator.parseInputNumber(ctrls[0]));
                    }
                };

            return $delegate;
        }
        private static parseInputNumber(ctrl: ng.INgModelController){
            return (viewValue: any) => {
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
            }
        }
    }
}