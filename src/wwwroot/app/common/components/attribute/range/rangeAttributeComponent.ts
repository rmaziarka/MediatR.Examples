/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('rangeAttribute', {
        controller: 'rangeAttributeController',
        controllerAs: 'vm',
        template: '<div ng-include="vm.getTemplateUrl()"></div>',
        bindings: {
            minValue: '=',
            maxValue: '=',
            label: '=',
            attribute: '=',
            mode: '@'
        }
    });
}