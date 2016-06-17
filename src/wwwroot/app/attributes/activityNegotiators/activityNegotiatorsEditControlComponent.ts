/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('activityNegotiatorsEditControl', {
        templateUrl: 'app/attributes/activityNegotiators/activityNegotiatorsEditControl.html',
        controllerAs: 'vm',
        controller: 'ActivityNegotiatorsEditControlController',
        bindings: {
            activityId: '<',
            propertyDivisionId: '@',
            leadNegotiator: '=' ,
            secondaryNegotiators: '=',
            onNegotiatorAdded: '&'
        }
    });
}