/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('busy', {
        controller: 'busyController',
        controllerAs: 'vm',
        templateUrl: 'app/common/components/busy/busy.html',
        bindings: {
            isBusy: '<',
            label: '@'
        }
    });
}