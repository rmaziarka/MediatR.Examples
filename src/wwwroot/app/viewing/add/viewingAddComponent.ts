/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('viewingAdd', {
        controller: 'viewingAddController',
        controllerAs : 'vm',
        templateUrl: 'app/viewing/add/viewingAdd.html',
        transclude : true,
        bindings : {
            componentId: '<',
            attendees: '<',
            requirement: '=',
        }
    });
}