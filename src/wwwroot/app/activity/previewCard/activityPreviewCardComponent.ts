/// <reference path='../../typings/_all.d.ts' />

module Antares.Attributes {
    angular.module('app').component('activityPreviewCard', {
        templateUrl:'app/activity/previewCard/activityPreviewcard.html',
            controllerAs: 'vm',
            controller: 'activityPreviewCardController',
            bindings: {
                config: '<',
                activity:'<',
                propertyTypeId:'<'
            }
    });
}