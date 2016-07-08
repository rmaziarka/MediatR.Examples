/// <reference path="../typings/_all.d.ts" />

module Antares.Tenancy {
    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.tenancy-view', {
                url: '/tenancy/view',
                params: {},
                template: '<tenancy-view></tenancy-view>'
            });
    }
}