/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('activityAdd', {
        templateUrl: 'app/activity/add/activityAdd.html',
        controllerAs: 'vm',
        controller: 'ActivityAddController',
        transclude : true,
        bindings: {
            componentId: '<'
        }
    });
}