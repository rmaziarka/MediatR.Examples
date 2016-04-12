/// <reference path="../../typings/_all.d.ts" />

module Antares.Property {
    angular.module('app').component('activityPreview', {
        templateUrl: 'app/activity/preview/activityPreview.html',
        controllerAs: 'vm',
        controller: 'ActivityPreviewController',
        bindings: {
            componentId: '<'
        }
    });
}