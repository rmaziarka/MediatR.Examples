/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('ownershipAdd', {
        templateUrl: 'app/ownership/add/ownershipAdd.html',
        controllerAs: 'vm',
        controller: 'OwnershipAddController',
        transclude : true,
        bindings: {
            componentId: '<',
            ownerships: '='
        }
    });
}