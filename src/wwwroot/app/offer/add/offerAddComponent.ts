/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('offerAdd', {
        controller: 'offerAddController',
        controllerAs : 'vm',
        templateUrl: 'app/offer/add/offerAdd.html',
        transclude : true,
        bindings : {
            componentId: '<',
            requirement: '<'
        }
    });
}