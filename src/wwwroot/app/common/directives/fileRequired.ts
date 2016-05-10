/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class FileRequiredDirective implements ng.IDirective {
        restrict = "E";
        require = 'ngModel';
        scope = {
            ngModel: '=ngModel'
        };
        link(scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ngModel: ng.INgModelController) {    
            
            var validateFileRequired =(inputValue: any) => {
                var isFileCleared = JSON.parse(String(attrs['isFileCleared']));
                var isValid = !isFileCleared || inputValue !== null;     
                
                ngModel.$setValidity('fileRequired', isValid);
                return isValid;
            }   
                 
            ngModel.$validators['fileRequired'] = (modelValue, scope) => {
                return validateFileRequired(modelValue);
            };

           
            attrs.$observe('isFileCleared', function () {
                validateFileRequired(ngModel.$viewValue);
            });
            
            scope.$watch('ngModel', (newValue, oldValue) => {
                if ((newValue !== undefined || oldValue !== undefined) && newValue !== oldValue) {
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