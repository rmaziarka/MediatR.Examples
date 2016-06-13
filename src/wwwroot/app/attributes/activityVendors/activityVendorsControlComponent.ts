/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('activityVendorsControl', {
        templateUrl: 'app/attributes/activityVendors/activityVendorsControl.html',
        controllerAs: 'vm',
        controller: 'ActivityVendorsControlController',
        bindings: {
            vendorContacts: '<',
            activityTypeId: '<',
            config: '<'
        }
    });
}