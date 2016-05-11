/// <reference path="../../typings/_all.d.ts" />
namespace Antares.Common.Directive {
    declare var moment:any;
    class TimeLowerThan {
        constructor() {
        }

        require = 'ngModel';
        link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: ng.IControllerService) => {
            var ngModel: ng.INgModelController = element.controller('ngModel');
            var validateDateRange = (inputValue: string) => {

                if (!inputValue || !attrs['timeLowerThan']) {
                    ngModel.$setValidity('timeLowerThan', true);
                    return inputValue;
                }
                var fromDate = moment(inputValue);
                var toDate = moment(attrs['timeLowerThan'].replace(/\"/g,""));
                var isValid = moment(fromDate).isBefore(toDate,'minute');
                ngModel.$setValidity('timeLowerThan', isValid);
                return inputValue;
            };

            ngModel.$parsers.push(validateDateRange);
            ngModel.$formatters.push(validateDateRange);
            attrs.$observe('timeLowerThan', function () {
                validateDateRange(ngModel.$viewValue);
            });
        }

        static factory() {
            var directive = () => {
                return new TimeLowerThan();
            };

            directive['$inject'] = [];
            return directive;
        }
    }

    angular.module('app').directive('timeLowerThan', TimeLowerThan.factory());
}