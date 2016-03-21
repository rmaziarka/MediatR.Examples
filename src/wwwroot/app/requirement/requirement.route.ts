/// <reference path="../typings/_all.d.ts" />

module Antares.Requirement {
    var app: ng.IModule = angular.module('app.requirement');

    app.config(initRoute);

    function initRoute($stateProvider) {
        $stateProvider
            .state('app.requirement-add', {
                url: '/requirement/add',
                template: '<requirement-add></requirement-add>'
            })
            .state('app.requirement-view', {
                url: '/requirement/:id',
                template: '<requirement-view></requirement-view>'
            });
    }
}