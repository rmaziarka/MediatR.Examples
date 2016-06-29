/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('activityAttendeesEditControl', {
        templateUrl: 'app/attributes/activityAttendees/activityAttendeesEditControl.html',
        controllerAs: 'vm',
        controller: 'ActivityAttendeesEditControlController',
        bindings: {
            users: '<',
            contacts: '<',
            attendees: '=',
            config: '<'
        }
    });
}