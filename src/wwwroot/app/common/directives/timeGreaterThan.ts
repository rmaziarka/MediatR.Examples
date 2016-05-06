/// <reference path="../../typings/_all.d.ts" />
namespace Antares.Common.Directive {
    
    declare var moment:any;
    class TimeGreaterThan {
        require: string = 'ngModel';

        constructor() {
        }

        link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: ng.IControllerService) => {
            var ngModel: ng.INgModelController = element.controller('ngModel');
            var validateDateRange = (inputValue: string): string => {
                if (!inputValue || !attrs['timeGreaterThan']) {
                    ngModel.$setValidity('timeGreaterThan', true);
                    return inputValue;
                }

                var fromDate: Date = moment(attrs['timeGreaterThan'].replace(/\"/g,""));
                var toDate: Date = moment(inputValue);
                var isValid: boolean = moment(fromDate).isBefore(toDate);
                ngModel.$setValidity('timeGreaterThan', isValid);
                return inputValue;
            };

            ngModel.$parsers.push(validateDateRange);
            ngModel.$formatters.push(validateDateRange);
            attrs.$observe('timeGreaterThan', () => {
                validateDateRange(ngModel.$viewValue);
            });
        }
        static factory() {
            var directive = (): TimeGreaterThan => {
                return new TimeGreaterThan();
            };

            directive['$inject'] = [];
            return directive;
        }
    }

    angular.module('app').directive('timeGreaterThan', TimeGreaterThan.factory());
}