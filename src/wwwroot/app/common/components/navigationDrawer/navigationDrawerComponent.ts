/// <reference path="../../../typings/_all.d.ts" />

module Antares.Common.Component {
    angular.module('app').component('navigationDrawer', {
        controller: 'navigationDrawerController',
        controllerAs: 'vm',
        templateUrl: 'app/common/components/navigationDrawer/navigationDrawer.html',
        bindings: {
            titleKey: '@',
            drawerFieldName: '@'
        }
    });
}