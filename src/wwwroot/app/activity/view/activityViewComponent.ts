/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('activityView', {
        templateUrl: 'app/activity/view/activityView.html',
        controllerAs: 'avvm',
        controller: 'activityViewController',
        bindings: {
            activity: '<',
            config: '<'
        }
    });
}