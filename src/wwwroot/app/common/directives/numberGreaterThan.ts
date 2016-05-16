/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class NumberGreaterThan implements ng.IDirective {
        restrict = 'A';
        require = 'ngModel';
        scope = {
            ngModel: '=ngModel'
        };

        link(scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ngModel: ng.INgModelController) {
            var greatedThanValue: number = parseInt(attrs['numberGreaterThan']);

            ngModel.$validators['numberGreaterThan'] = (modelValue: number) => {
                if (modelValue === null || modelValue === undefined) {
                    return true;
                }

                return (modelValue > greatedThanValue);
            };
        };

        static factory() {
            var directive = () => {
                return new NumberGreaterThan();
            };
            directive['$inject'] = [];
            return directive;
        }
    }

    angular.module('app').directive('numberGreaterThan', NumberGreaterThan.factory());
}