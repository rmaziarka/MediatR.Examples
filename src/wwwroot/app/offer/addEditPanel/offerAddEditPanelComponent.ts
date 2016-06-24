/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('offerAddEditPanel', {
        controller: 'offerAddEditPanelController',
        controllerAs : 'vm',
        templateUrl: 'app/offer/addEditPanel/offerAddPanel.html',
        transclude : true,
        bindings: {
            isVisible: '<',
            activity: '<',
            requirement: '<',
            mode: '@'
        }
    });
}