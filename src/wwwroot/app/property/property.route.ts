/// <reference path="../typings/_all.d.ts" />

module Antares.Property {
    var app: ng.IModule = angular.module('app');

    app.config(initRoute);

    function initRoute($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider) {
        $stateProvider
            .state('app.property-view', {
                url: '/property/view/:id',
                params: {},
                template: '<property-view></property-view>'
            })
            .state('app.property-edit', {
                url: '/property/edit/:id',
                params: {},
                template: '<property-edit></property-edit>'
            })
            .state('app.property-add', {
                url: '/property/add',
                params: {},
                template: '<property-add></property-add>'
            });

    }
}