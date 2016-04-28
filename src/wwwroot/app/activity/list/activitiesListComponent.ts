/// <reference path="../../typings/_all.d.ts" />

module Antares.Component {
    angular.module('app').component('activitiesList', {
        controller: 'activitiesListController',
        controllerAs : 'vm',
        templateUrl : 'app/activity/list/activitiesList.html',
        transclude : true,
        bindings : {
            componentId: '<'
        }
    });
}