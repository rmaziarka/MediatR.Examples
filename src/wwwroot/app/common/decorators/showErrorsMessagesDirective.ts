/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class FormShowErrorsMessagesDirective implements ng.IDirective {
        restrict = 'A';
        require = '^form';
        link: any = {};

        constructor() {
            this.link = this.unboundLink.bind(this);
        }

        unboundLink(scope: ng.IScope, el: ng.IAugmentedJQuery, attrs: ng.IAttributes[], formCtrl: ng.IFormController) {
            var formControlElement = el[0].parentElement.querySelector('.form-control[name]');
            var ngFormControlElement = angular.element(formControlElement);
            var formControlName = ngFormControlElement.attr('name');

            scope.$watch(() => {
                return formCtrl[formControlName].$error;
            }, (showErrors: string) => {
                el.attr('ng-messages', showErrors);
            })

            scope.$watch(() => {
                return formCtrl.$submitted || formCtrl[formControlName].$dirty;
            }, (showErrors: string) => {
                el.attr('ng-show', showErrors);
            });
        }

        static factory() {
            var directive = ($interpolate: ng.IInterpolateService) => {
                return new FormShowErrorsMessagesDirective();
            };

            return directive;
        }

    }

    angular.module('app').directive('showErrorsMessages', FormShowErrorsMessagesDirective.factory());
}
