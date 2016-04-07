/// <reference path="../../typings/_all.d.ts" />

module Antares {
    export module Common {
        export module Directive {
            export class KfMaxDateDirective implements ng.IDirective {
                static InvalidDateErrorMessage ="kf-max-date: Max date ${maxDate} provided by user is invalid" 
                restrict = 'A';
                link(scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) {
                    var ngModel: ng.INgModelController = element.controller('ngModel'),
                        maxDate = attrs['kfMaxDate'];

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

                    ngModel.$validators['kfMaxDate'] = (modelValue : string) => {

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