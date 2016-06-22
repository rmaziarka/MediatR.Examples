/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityPreviewPanel', {
        templateUrl: 'app/activity/previewPanel/activityPreviewPanel.html',
        controllerAs: 'vm',
        controller: 'activityPreviewPanelController',
        bindings: {
            isVisible: '<',
            activity: '<',
            propertyTypeId:'<'
        }
    });
}