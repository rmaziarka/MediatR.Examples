/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class FileRequiredDirective implements ng.IDirective {
        restrict = "A";
        require = 'ngModel';

        link(scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ngModel: ng.INgModelController) {
            var validateFileRequired =(inputValue: any) => {
                var isValid = !!inputValue;

                ngModel.$setValidity('fileRequired', isValid);

                return isValid;
            }

            ngModel.$validators['fileRequired'] = (modelValue) => {
                return validateFileRequired(modelValue);
            };
        };

        static factory() {
            var directive = () => {
                return new FileRequiredDirective();
            };

            directive['$inject'] = [];
            return directive;
        }
    }

    angular.module('app').directive('fileRequired', FileRequiredDirective.factory());
}