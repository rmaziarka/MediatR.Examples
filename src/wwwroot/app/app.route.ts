/// <reference path="typings/_all.d.ts" />

module Antares {
    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise('/app');
    }
}