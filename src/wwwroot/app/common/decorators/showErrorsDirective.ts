
/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class FormShowErrorsDirective implements ng.IDirective {
        restrict = 'A';
        require = '^form';
        link: any = {};

        constructor(private $interpolate: ng.IInterpolateService) {
            this.link = this.unboundLink.bind(this);
        }

        unboundLink(scope: ng.IScope, el: any, attrs: ng.IAttributes[], formCtrl: ng.IFormController) {
            var formControlElement = el[0].querySelector('.form-control[name]');
            var ngFormControlElement = angular.element(formControlElement);
            var formControlName = this.$interpolate(ngFormControlElement.attr('name') || '')(scope);

            scope.$watch(() => {
                return (formCtrl.$submitted || formCtrl[formControlName].$dirty) && formCtrl[formControlName].$invalid;
            }, (hasErrors: string) => {
                el.toggleClass('has-error', hasErrors);
            });
        }

        static factory() {
            var directive = ($interpolate: ng.IInterpolateService) => {
                return new FormShowErrorsDirective($interpolate);
            };

            directive['$inject'] = ['$interpolate']
            return directive;
        }
    }

    angular.module('app').directive('showErrors', FormShowErrorsDirective.factory());
}
