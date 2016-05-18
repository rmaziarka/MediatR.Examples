/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    class FocusOnDirective implements ng.IDirective {
        restrict = "A";

        constructor(private $timeout: ng.ITimeoutService) {
        }

        link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
            scope.$watch(attrs['focusOn'], (currentValue) => {
                if (currentValue === true) {
                    this.$timeout(() => {
                        if (element.is(':input')) {
                            element.focus();
                        }
                        else {
                            element.find(':input:first').focus();
                        }
                    });
                }
            });
        };

        static factory() {
            var directive = ($timeout: any): FocusOnDirective => {
                return new FocusOnDirective($timeout);
            };

            directive['$inject'] = ['$timeout'];
            return directive;
        }
    }


    angular.module('app').directive('focusOn', FocusOnDirective.factory());
}