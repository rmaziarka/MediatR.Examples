/// <reference path="../../typings/_all.d.ts" />

module Antares {
    export module Common {
        export module Directive {
            export class KfMaxDateDirective implements ng.IDirective {
                static InvalidDateErrorMessage = "kf-max-date: Max date ${maxDate} provided by user is invalid"
                restrict = 'A';
                require = 'ngModel';
                link(scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: ng.IControllerService) {
                    var ngModel: ng.INgModelController = element.controller('ngModel'),
                        maxDate: string = attrs['kfMaxDate'];

                    if (maxDate === '') {
                        return;
                    }

                    if (isNaN(Date.parse(maxDate))) {
                        throw new Error(`kf-max-date: Max date ${maxDate} provided by user is invalid`);
                    }

                    var maxAllowedDate = Antares.Core.DateTimeUtils.convertDateToUtc(maxDate);

                    var setValidation = (isValid: boolean): boolean => {
                        ngModel.$setValidity('kfMaxDate', isValid);
                        return isValid;
                    }

                    ngModel.$validators['kfMaxDate'] = (modelValue: string) => {

                        if (modelValue === null || modelValue === '') {
                            return setValidation(true);
                        }

                        var modelDate = Antares.Core.DateTimeUtils.convertDateToUtc(modelValue);
                        var isValid = modelDate <= maxAllowedDate;

                        return setValidation(isValid);
                    }
                };

                static factory() {
                    var directive = () => {
                        return new KfMaxDateDirective();
                    };

                    directive['$inject'] = [];
                    return directive;
                }
            }

            angular.module('app').directive('kfMaxDate', KfMaxDateDirective.factory());
        }
    }
}

namespace Antares.Common.Directive {

    var isValidDate = function(dateStr) {
        if (dateStr == undefined)
            return false;
        var dateTime = Date.parse(dateStr);

        if (isNaN(dateTime)) {
            return false;
        }
        return true;
    };

    var getDateDifference = function(fromDate, toDate) {
        return Date.parse(toDate) - Date.parse(fromDate);
    };

    var isValidDateRange = function(fromDate, toDate) {
        if (fromDate == "" || toDate == "")
            return true;
        if (isValidDate(fromDate) == false) {
            return false;
        }
        if (isValidDate(toDate) == true) {
            var days = getDateDifference(fromDate, toDate);
            if (days < 0) {
                return false;
            }
        }
        return true;
    };

    export class DateGreaterThan {
        require = 'ngModel';

        constructor() {

        }
        link = function(scope, element, attrs, ctrl) {
            var ngModel: ng.INgModelController = element.controller('ngModel');
            var validateDateRange = function(inputValue) {

                if (!inputValue || !attrs.dateGreaterThan) {
                    ngModel.$setValidity('dateGreaterThan', true);
                    return true;
                }

                var fromDate = Antares.Core.DateTimeUtils.createDateAsUtc(attrs.dateGreaterThan);
                var toDate = Antares.Core.DateTimeUtils.createDateAsUtc(inputValue);
                var isValid = isValidDateRange(fromDate, toDate);
                ngModel.$setValidity('dateGreaterThan', isValid);
                return inputValue;
            };

            ngModel.$parsers.unshift(validateDateRange);
            ngModel.$formatters.push(validateDateRange);
            attrs.$observe('dateGreaterThan', function() {
                validateDateRange(ngModel.$viewValue);

            });
        }
        static factory() {
            var directive = () => {
                return new DateGreaterThan();
            };

            directive['$inject'] = [];
            return directive;
        }
    }

    angular.module('app').directive('dateGreaterThan', DateGreaterThan.factory());

    export class DateLowerThan {
        constructor() {

        }
        require = 'ngModel';
        link = function(scope, element, attrs, ctrl) {
            var ngModel: ng.INgModelController = element.controller('ngModel');
            var validateDateRange = function(inputValue) {
                
                
                if (!inputValue || !attrs.dateLowerThan) {
                    ngModel.$setValidity('dateLowerThan', true);
                    return true;
                }

                var fromDate = Antares.Core.DateTimeUtils.createDateAsUtc(inputValue);
                var toDate = Antares.Core.DateTimeUtils.createDateAsUtc(attrs.dateLowerThan);
                var isValid = isValidDateRange(fromDate, toDate);
                ngModel.$setValidity('dateLowerThan', isValid);
                return inputValue;
            };

            ngModel.$parsers.unshift(validateDateRange);
            ngModel.$formatters.push(validateDateRange);
            attrs.$observe('dateLowerThan', function() {
                validateDateRange(ngModel.$viewValue);

            });
        }
          static factory() {
            var directive = () => {
                return new DateLowerThan();
            };

            directive['$inject'] = [];
            return directive;
        }
    }

    angular.module('app').directive('dateLowerThan',DateLowerThan.factory())

}