/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    class BlurOnDirective implements ng.IDirective {
        restrict = "A";

        constructor(private $timeout: ng.ITimeoutService) {
        }

        link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) => {
            scope.$watch(attrs['blurOn'], (currentValue) => {
                if (currentValue === true) {
                    this.$timeout(() => {
                        if (element.is(':input')) {
                            element.blur();
                        }
                        else {
                            element.find(':input:first').blur();
                        }
                    });
                }
            });
        };

        static factory() {
            var directive = ($timeout: any): BlurOnDirective => {
                return new BlurOnDirective($timeout);
            };

            directive['$inject'] = ['$timeout'];
            return directive;
        }
    }


    angular.module('app').directive('blurOn', BlurOnDirective.factory());
}