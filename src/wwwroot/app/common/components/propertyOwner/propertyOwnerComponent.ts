/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('propertyOwner', {
        controller: 'propertyOwnerController',
        controllerAs: 'vm',
        templateUrl: 'app/common/components/propertyOwner/propertyOwner.html',
        bindings: {
            ownerships: '<'
        }
    });
}