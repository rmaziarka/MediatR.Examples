/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('viewingDetails', {
        controller: 'viewingDetailsController',
        controllerAs : 'vm',
        templateUrl: 'app/viewing/details/viewingDetails.html',
        transclude : true,
        bindings : {
            componentId: '<',
            attendees: '<'
        }
    });
}