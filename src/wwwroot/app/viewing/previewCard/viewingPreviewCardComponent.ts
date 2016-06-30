/// <reference path="../../typings/_all.d.ts" />

module Antares.Viewing {
    angular.module('app').component('viewingPreviewCard', {
        templateUrl: 'app/viewing/previewCard/viewingPreviewCard.html',
        controller: 'ViewingPreviewCardController',
        controllerAs: 'vm',
        bindings: {
            viewing: '<'
        }
    });
}