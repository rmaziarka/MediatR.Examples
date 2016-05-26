/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('offerAdd', {
        controller: 'offerAddEditController',
        controllerAs : 'vm',
        templateUrl: 'app/offer/addEdit/offerAdd.html',
        transclude : true,
        bindings : {
            componentId: '<',
            requirement: '<',
            mode: '@'
        }
    });

    angular.module('app').component('offerEdit', {
        controller: 'offerAddEditController',
        controllerAs : 'vm',
        templateUrl: 'app/offer/addEdit/offerEdit.html',
        transclude : true,
        bindings : {
            componentId: '<',
            requirement: '<',
            mode: '@'
        }
    });
}