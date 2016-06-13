/// <reference path="../typings/_all.d.ts" />

module Antares.Preferences {
    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider) {
        $stateProvider
            .state('app.preferences', {
                url: '/preferences',
                params: {},
                template: '<preferences></preferences>'
            });
    }
}
