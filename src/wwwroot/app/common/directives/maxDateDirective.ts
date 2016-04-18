/// <reference path="../../typings/_all.d.ts" />

module Antares {
    export module Common {
        export module Directive {
            export class KfMaxDateDirective implements ng.IDirective {
                static InvalidDateErrorMessage = "kf-max-date: Max date ${maxDate} provided by user is invalid"
                restrict = 'A';
                require = 'ngModel';
                link: any = {};
                constructor(private uibDateParser: any) {
                    this.link = this.unboundLink.bind(this);
                }
                unboundLink(scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: ng.IControllerService) {
                    var ngModel: ng.INgModelController = element.controller('ngModel'),
                        maxDate: string = attrs['kfMaxDate'];

                    if (maxDate === '') {
                        return maxDate;
                    }

                    var maxAllowedDate = this.uibDateParser.parse(maxDate, 'dd-MM-yyyy');

                    var setValidation = (isValid: boolean): boolean => {
                        ngModel.$setValidity('kfMaxDate', isValid);
                        return isValid;
                    }

                    var validateDate = (modelValue: any) => {

                        if (modelValue === null || modelValue === '' || modelValue.toString() == NaN.toString()) {
                            setValidation(true);
                            return modelValue;
                        }

                        var modelDate = this.uibDateParser.parse(modelValue, 'dd-MM-yyyy');
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
                    var directive = (uibDateParser: any) => {
                        return new KfMaxDateDirective(uibDateParser);
                    };

                    directive['$inject'] = ['uibDateParser'];
                    return directive;
                }
            }

            angular.module('app').directive('kfMaxDate', KfMaxDateDirective.factory());
        }
    }
} 