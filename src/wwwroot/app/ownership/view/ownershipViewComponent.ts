/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('ownershipView', {
        templateUrl: 'app/ownership/view/ownershipView.html',
        controllerAs: 'vm',
        controller: 'OwnershipViewController',
        transclude : true,
        bindings: {
            componentId: '<'
        }
    });
}