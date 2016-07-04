/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('viewingViewControl', {
        templateUrl: 'app/attributes/viewing/viewingViewControl.html',
        controllerAs: 'vm',
        controller: 'ViewingViewControlController',
        bindings: {
            viewings: '<',            
            isPanelVisible: '<',
            config: '<'
        }
    });
}