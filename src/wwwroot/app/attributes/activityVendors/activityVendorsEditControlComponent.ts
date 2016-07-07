/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('activityVendorsEditControl', {
        templateUrl: 'app/attributes/activityVendors/activityVendorsEditControl.html',
        controllerAs: 'vm',
        controller: 'ActivityVendorsEditControlController',
        bindings: {
            contacts: '<',
            config: '<'
        }
    });
}