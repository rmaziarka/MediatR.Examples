/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('offerAddEdit', {
        controller: 'offerAddEditController',
        controllerAs : 'vm',
        templateUrl: 'app/offer/addEdit/offerAddEdit.html',
        transclude : true,
        bindings : {
            componentId: '<',
            requirement: '<'
        }
    });
}