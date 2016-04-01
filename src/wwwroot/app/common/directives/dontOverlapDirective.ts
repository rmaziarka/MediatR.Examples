/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    export class DontOverlapDirective implements ng.IDirective {
        restrict = 'A';
        controller = 'dontOverlapController';

        link(scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, controller: DontOverlapController) {
            var ngModel: ng.INgModelController = element.controller('ngModel');

            controller.attachAttributeObservers(attrs, ngModel);
            controller.initModelValidationHandlers(attrs, ngModel);
        };

        static factory() {
            var directive = () => {
                return new DontOverlapDirective();
            };

            directive['$inject'] = [];
            return directive;
        }
    }

    angular.module('app').directive('dontOverlap', DontOverlapDirective.factory());
}