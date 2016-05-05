/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class FileRequiredDirective implements ng.IDirective {
        restrict = "A";
        require = 'ngModel';        
        scope = {
            ngModel: '=ngModel'
        };
        link(scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ngModel: ng.INgModelController) {

            ngModel.$validators['fileRequired'] = (modelValue) => {
                return modelValue !== null && modelValue.name.length > 0;
            };

            scope.$watch('ngModel', (newValue, oldValue) => {
                if (newValue !== oldValue) {                    
                    ngModel.$setDirty();
                }
            });
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