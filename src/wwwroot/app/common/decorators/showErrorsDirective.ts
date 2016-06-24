
/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class FormShowErrorsDirective implements ng.IDirective {
        restrict = 'A';
        require = '^form';
        compile: any = {}; 

        constructor() {
            this.compile = (tElement: any, tAttrs: any, transclude: any) => {
                return {
                    pre: function preLink(scope: any, iElement: any, iAttrs: any, controller: any) {
                    },
                    post: function postLink(scope: ng.IScope, el: any, iAttrs: any, formCtrl: ng.IFormController) {
                        var formControlElement = el[0].querySelector('.form-control[name]');
                        var ngFormControlElement = angular.element(formControlElement);
                        var formControlName = ngFormControlElement.attr('name');

                        scope.$watch(() => {
                            return (formCtrl.$submitted || formCtrl[formControlName].$dirty) && formCtrl[formControlName].$invalid;
                        }, (hasErrors: string) => {
                            el.toggleClass('has-error', hasErrors);
                        });
                    }
                }
            }
        }

        static factory() {
            var directive = () => {
                return new FormShowErrorsDirective();
            };

            return directive;
        }
    }

    angular.module('app').directive('showErrors', FormShowErrorsDirective.factory());
}
