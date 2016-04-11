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

                    var validateDate = (modelValue: string) => {

                        if (modelValue === null || modelValue === '') {
                            setValidation(true);
                            return modelValue;
                        }

                        var modelDate = Antares.Core.DateTimeUtils.convertDateToUtc(modelValue);
                        var isValid = modelDate <= maxAllowedDate;

                        setValidation(isValid);
                        return modelValue;
                    }

                    ngModel.$parsers.push(validateDate);
                    ngModel.$formatters.push(validateDate);
                    attrs.$observe('kfMaxDate', () => {
                        validateDate(ngModel.$viewValue);

                    });
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

    var isValidDate = (date: Date): boolean => {
        if (date == undefined)
            return false;
        var dateTime: number = Date.parse(date.toString());

        if (isNaN(dateTime)) {
            return false;
        }
        return true;
    };

    var getDateDifference = (fromDate: Date, toDate: Date): number => {
        return Date.parse(toDate.toDateString()) - Date.parse(fromDate.toDateString());
    };

    var isValidDateRange = (fromDate: Date, toDate: Date): boolean => {
        if (!fromDate || !toDate)
            return true;
        if (isValidDate(fromDate) == false) {
            return false;
        }
        if (isValidDate(toDate) == true) {
            var days: number = getDateDifference(fromDate, toDate);
            if (days < 0) {
                return false;
            }
        }
        return true;
    };

    export class DateGreaterThan {
        require: string = 'ngModel';

        constructor(private $filter: ng.IFilterService, private uibDateParser: any) {

        }
        link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: ng.IControllerService) => {
            var ngModel: ng.INgModelController = element.controller('ngModel');
            var validateDateRange = (inputValue: string): string => {

                if (!inputValue || !attrs['dateGreaterThan']) {
                    ngModel.$setValidity('dateGreaterThan', true);
                    return inputValue;
                }

                var fromDate: Date = Antares.Core.DateTimeUtils.createDateAsUtc(attrs['dateGreaterThan']);
                var toDate: Date = this.uibDateParser.parse(inputValue, 'dd-MM-yyyy', 0);
                var isValid: boolean = isValidDateRange(fromDate, toDate);
                ngModel.$setValidity('dateGreaterThan', isValid);
                return inputValue;
            };

            ngModel.$parsers.push(validateDateRange);
            ngModel.$formatters.push(validateDateRange);
            attrs.$observe('dateGreaterThan', () => {
                validateDateRange(ngModel.$viewValue);
            });
        }
        static factory() {
            var directive = ($filter: ng.IFilterService, uibDateParser: any): DateGreaterThan => {
                return new DateGreaterThan($filter, uibDateParser);
            };

            directive['$inject'] = ["$filter", "uibDateParser"];
            return directive;
        }
    }

    angular.module('app').directive('dateGreaterThan', DateGreaterThan.factory());

    export class DateLowerThan {
        constructor(private $filter: ng.IFilterService, private uibDateParser: any) {
        }

        require = 'ngModel';
        link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: ng.IControllerService) => {
            var ngModel: ng.INgModelController = element.controller('ngModel');
            var validateDateRange = (inputValue: string) => {

                if (!inputValue || !attrs['dateLowerThan']) {
                    ngModel.$setValidity('dateLowerThan', true);
                    return inputValue;
                }
                var fromDate = this.uibDateParser.parse(inputValue, 'dd-MM-yyyy', 0);
                var toDate = Antares.Core.DateTimeUtils.createDateAsUtc(attrs['dateLowerThan']);
                var isValid = isValidDateRange(fromDate, toDate);
                ngModel.$setValidity('dateLowerThan', isValid);
                return inputValue;
            };

            ngModel.$parsers.push(validateDateRange);
            ngModel.$formatters.push(validateDateRange);
            attrs.$observe('dateLowerThan', function() {
                validateDateRange(ngModel.$viewValue);
            });
        }

        static factory() {
            var directive = ($filter: ng.IFilterService, uibDateParser: any) => {
                return new DateLowerThan($filter, uibDateParser);
            };

            directive['$inject'] = ["$filter", "uibDateParser"];
            return directive;
        }
    }

    angular.module('app').directive('dateLowerThan', DateLowerThan.factory());
}