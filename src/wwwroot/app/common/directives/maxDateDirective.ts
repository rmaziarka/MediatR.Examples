/// <reference path="../../typings/_all.d.ts" />

module Antares {
    export module Common {
        export module Directive {
            export class KfMaxDateDirective implements ng.IDirective {
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

                    var maxAllowedDate = new Date(maxDate);

                    var setValidation = (isValid: boolean): boolean => {
                        ngModel.$setValidity('kfMaxDate', isValid);
                        return isValid;
                    }

                    ngModel.$validators['kfMaxDate'] = (modelValue) => {

                        if (modelValue === null) {
                            return setValidation(true);
                        }

                        var modelDate = new Date(modelValue);
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