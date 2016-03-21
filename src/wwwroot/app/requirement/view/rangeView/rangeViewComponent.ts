/// <reference path="../../../typings/_all.d.ts" />

module Antares.Requirement.View {
    angular.module('app').component('rangeView', {
        controller: 'rangeViewController',
        controllerAs: 'vm',
        templateUrl: 'app/requirement/view/rangeView/rangeView.html',
        transclude: true,
        bindings: {
            label: '@',
            min: '<',
            max: '<',
            suffix: '<'
        }
    });
}