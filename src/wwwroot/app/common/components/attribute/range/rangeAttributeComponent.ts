/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('rangeAttribute', {
        controller: 'rangeAttributeController',
        controllerAs: 'vm',
        templateUrl: 'app/common/components/attribute/range/rangeAttribute.html',
        bindings: {
            minValue: '=',
            maxValue: '=',
            label: '=',
            attribute: '='
        }
    });
}