/// <reference path="../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('activityNegotiatorsViewControl', {
        templateUrl: 'app/attributes/activityNegotiators/activityNegotiatorsViewControl.html',
        controllerAs: 'vm',
        controller: 'ActivityNegotiatorsViewControlController',
        bindings: {
            leadNegotiator: '<',
            canBeEdited: '<',
            secondaryNegotiators: '<',
            hideSecondaryNegotiators: '<',
            propertyDivisionId: '@',
			config:'<'
        }
    });
}