/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('activityAddPanel', {
        templateUrl: 'app/activity/addPanel/activityAddPanel.html',
        controllerAs: 'vm',
        controller: 'ActivityAddPanelController',
        bindings: {
            isVisible: '<',
            data: '<'
        }
    });
}