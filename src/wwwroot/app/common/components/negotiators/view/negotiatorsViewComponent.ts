/// <reference path="../../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('negotiatorsView', {
        templateUrl: 'app/common/components/negotiators/view/negotiatorsView.html',
        controllerAs: 'nvvm',
        controller: 'NegotiatorsController',
        bindings: {
            leadNegotiator: '<',
            secondaryNegotiators: '<',
            propertyDivisionId: '@'
        }
    });
}