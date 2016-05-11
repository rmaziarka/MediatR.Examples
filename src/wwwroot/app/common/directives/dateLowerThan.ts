/// <reference path="../../typings/_all.d.ts" />
namespace Antares.Common.Directive {
    class DateLowerThan {
        constructor(private uibDateParser: any, private validDateRange: Antares.Core.Service.ValidDateRange) {
        }

        require = 'ngModel';
        link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: ng.IControllerService) => {
            var ngModel: ng.INgModelController = element.controller('ngModel');
            var validateDateRange = (inputValue: string) => {

                if (!inputValue || !attrs['dateLowerThan']) {
                    ngModel.$setValidity('dateLowerThan', true);
                    return inputValue;
                }
                
                var toDateString : string = JSON.parse(attrs['dateLowerThan']);
                var fromDate : Date = this.uibDateParser.parse(inputValue, 'dd-MM-yyyy');
                var toDate: Date = new Date(toDateString);
                var isValid = this.validDateRange.isValidDateRange(fromDate, toDate);
                ngModel.$setValidity('dateLowerThan', isValid);
                return inputValue;
            };

            ngModel.$parsers.push(validateDateRange);
            ngModel.$formatters.push(validateDateRange);
            attrs.$observe('dateLowerThan', function () {
                validateDateRange(ngModel.$viewValue);
            });
        }

        static factory() {
            var directive = (uibDateParser: any, validDateRange: Antares.Core.Service.ValidDateRange) => {
                return new DateLowerThan(uibDateParser, validDateRange);
            };

            directive['$inject'] = ["uibDateParser", "validDateRange"];
            return directive;
        }
    }

    angular.module('app').directive('dateLowerThan', DateLowerThan.factory());
}