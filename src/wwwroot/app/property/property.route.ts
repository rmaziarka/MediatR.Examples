/// <reference path="../typings/_all.d.ts" />

module Antares.Property {
    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('app.property-view', {
                url: '/property/view',
                params: {},
                template: '<property-view></property-view>'
            })
            .state('app.property-add', {
                url: '/property/add',
                params: {},
                template: '<property-add></property-add>'
            });

    }
}