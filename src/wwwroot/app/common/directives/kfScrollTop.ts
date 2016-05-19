/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Directive {
    class KfScrollTopDirective implements ng.IDirective {
        restrict = "E";
        scope = {
            ngModel: '=ngModel'
        };

        link = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes) =>{
            var inputElement = element.parent().find(attrs['forInputElement']);

            inputElement.on('input', () => {
                var elementToScroll = element.parent().find(attrs['scrollOnElement']);
                elementToScroll.scrollTop(0);
            });
        };

        static factory() {
            var directive = (): KfScrollTopDirective => {
                return new KfScrollTopDirective();
            };

            directive['$inject'] = [];
            return directive;
        }
    }


    angular.module('app').directive('kfScrollTop', KfScrollTopDirective.factory());
}