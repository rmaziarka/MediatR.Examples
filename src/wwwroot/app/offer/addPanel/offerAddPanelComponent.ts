/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('offerAddPanel', {
        controller: 'offerAddPanelController',
        controllerAs : 'vm',
        templateUrl: 'app/offer/addPanel/offerAddPanel.html',
        transclude : true,
        bindings: {
            isVisible: '<',
            activity: '<',
            requirement: '<'
        }
    });
}