/// <reference path="../../typings/_all.d.ts" />

module Antares.Attributes {
    angular.module('app').component('activityTypeEditControl', {
        templateUrl: 'app/attributes/activityType/activityTypeEditControl.html',
        controllerAs: 'vm',
        controller: 'ActivityTypeEditControlController',
        bindings: {
            propertyTypeId: '<',
            config: '<',
            ngModel: '=',
            onActivityTypeChanged: '&'
        }
    });
}